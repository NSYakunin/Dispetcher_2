using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using System.Net;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    class С_TreeViewOrders
    {
        IConfig config;
        
        public С_TreeViewOrders(IConfig config)
        {
            this.config = config;
        }

        //*****************************************************************************
        #region Формируем дерево заказа при загрузке формы заказа
        private DataTable DT_TreeViewOrders = new DataTable();

        /*public DataTable Get_DT_TreeView
        {
            get { return DT_TreeViewOrders; }
        }*/

        //С_TreeViewOrders C_TV_Orders = new С_TreeViewOrders();
        //C_TV_Orders.AddInTreeViewOrders(PK_IdOrder, OrderNum, OrderName, ref treeViewOrdersDetails);
        //if (treeViewOrdersDetails.SelectedNode == null) treeViewOrdersDetails.SelectedNode = treeViewOrdersDetails.TopNode;
        public void AddInTreeViewOrders(int PK_IdOrder, string OrderNum, string OrderName, ref TreeView TreeV, bool Plan)
        {
            TreeV.Nodes.Clear();
            TreeNode ParentNode = new TreeNode();
            ParentNode.Text = OrderNum + " - " + OrderName;
            ParentNode.Tag = 0;
            ParentNode.Name = OrderNum;
            TreeV.Nodes.Add(ParentNode);
            //**************************************************
            SelectInDT_TreeViewOrders(PK_IdOrder, Plan);
            if (DT_TreeViewOrders.Rows.Count > 0) LoadTreeMenu(ref TreeV);
            //***************************************************************************
            //DT_TreeViewOrders.Clear();

            /*ParentNode = new TreeNode();
            ParentNode.Text = OrderNum + " - " + OrderName;
            ParentNode.Tag = 0;
            TreeV.Nodes.Add(ParentNode);
            //**************************************************
            SelectInDT_TreeViewOrders(PK_IdOrder);
            if (DT_TreeViewOrders.Rows.Count > 0) LoadTreeMenu(ref TreeV);*/

        }

        private void SelectInDT_TreeViewOrders(int _PK_IdOrder, bool _Plan)
        {
            //DT_TreeViewOrders.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "";

                    if (_Plan) cmd.CommandText = "Select PK_IdOrderDetail,ShcmDetail,NameDetail,AmountDetails,Position,PositionParent,FK_IdTypeDetail" + "\n" +
                                      "From OrdersDetails" + "\n" +
                                      "left Join Sp_Details on Sp_Details.PK_IdDetail=OrdersDetails.FK_IdDetail" + "\n" +
                                      "Where FK_IdOrder = @PK_IdOrder and Position >=0" + "\n" +
                                      "Order by Position";
                    else
                        cmd.CommandText = "Select PK_IdOrderDetail,ShcmDetail,NameDetail,AmountDetails,Position,PositionParent,FK_IdTypeDetail" + "\n" +
                                          "From OrdersDetails" + "\n" +
                                          "left Join Sp_Details on Sp_Details.PK_IdDetail=OrdersDetails.FK_IdDetail" + "\n" +
                                          "Where FK_IdOrder = @PK_IdOrder" + "\n" +
                                          "union" + "\n" +
                                          "Select PK_IdFasteners,'К',NameFasteners,AmountFasteners,Position,PositionParent,FK_IdTypeFasteners as FK_IdTypeDetail" + "\n" +
                                          "From OrdersFasteners" + "\n" +
                                          "Where FK_IdOrder = @PK_IdOrder" + "\n" +
                                          "Order by Position";
                    cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@PK_IdOrder"].Value = _PK_IdOrder;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT_TreeViewOrders);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*private void LoadTreeMenu(ref TreeView TreeV)
        {
            if (DT_TreeViewOrders.Rows.Count > 0)
            {
                foreach (DataRow dRow in DT_TreeViewOrders.Select("PositionParent=0"))
                {
                    TreeNode ParentNode = new TreeNode();
                    int Position;
                    if (int.TryParse(dRow["Position"].ToString(), out Position))
                        ParentNode.Text = dRow["Position"].ToString() + " | " + dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                    else
                        ParentNode.Text = dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                    ParentNode.Tag = dRow["PK_IdOrderDetail"].ToString();
                    TreeV.Nodes.Add(ParentNode);
                    
                    if (int.TryParse(dRow["Position"].ToString(), out Position))
                        loadTreeSubMenu(ref ParentNode, Position, DT_TreeViewOrders);
                }
            }
        }*/

        private void LoadTreeMenu(ref TreeView TreeV)
        {
            if (DT_TreeViewOrders.Rows.Count > 0)
            {
                DataRow[] childs;
                foreach (DataRow dRow in DT_TreeViewOrders.Rows)
                {
                    int Position = 0;
                    if (!int.TryParse(dRow["Position"].ToString(), out Position)) Position = 0;
                    childs = DT_TreeViewOrders.Select("Position='" + dRow["PositionParent"].ToString() + "'");
                    if (dRow["PositionParent"].ToString() == "0" || childs.Length == 0)
                    {
                        TreeNode ParentNode = new TreeNode();

                        if (Position > 0)
                            ParentNode.Text = dRow["Position"].ToString() + " | " + dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                        else
                            ParentNode.Text = dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                        ParentNode.Tag = dRow["PK_IdOrderDetail"].ToString();
                        ParentNode.Name = dRow["FK_IdTypeDetail"].ToString();
                        TreeV.Nodes.Add(ParentNode);

                        if (int.TryParse(dRow["Position"].ToString(), out Position))
                            loadTreeSubMenu(ref ParentNode, Position, DT_TreeViewOrders);
                    }
                }
            }
        }

        private void loadTreeSubMenu(ref TreeNode _ParentNode, int ParentId, DataTable dtMenu)
        {
            DataRow[] childs = dtMenu.Select("PositionParent='" + ParentId + "'");
            foreach (DataRow dRow in childs)
            {
                TreeNode child = new TreeNode();
                int Position;
                if (int.TryParse(dRow["Position"].ToString(), out Position))
                    child.Text = dRow["Position"].ToString() + " | " + dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                else
                    child.Text = dRow["ShcmDetail"].ToString() + " - " + dRow["NameDetail"].ToString() + " (" + dRow["AmountDetails"].ToString() + ")";
                child.Tag = dRow["PK_IdOrderDetail"].ToString();
                child.Name = dRow["FK_IdTypeDetail"].ToString();
                _ParentNode.Nodes.Add(child);
                //Recursion Call
                if (int.TryParse(dRow["Position"].ToString(), out Position))
                    loadTreeSubMenu(ref child, Position, dtMenu);
            }
        }
        #endregion
        //*****************************************************************************


        public static void SearchTree(TreeNodeCollection _TnCollection, string _searchtext)
        {
            if (_searchtext.Length > 0)
                foreach (TreeNode node in _TnCollection)
                {
                    node.BackColor = Color.Empty;
                    if (node.Text.ToLower().Contains(_searchtext.ToLower()))
                    {
                        node.BackColor = Color.Gold;
                        node.EnsureVisible();
                    }
                    /*if (node.Tag as string == _searchtext)
                    {// return node;
                    }*/
                    SearchTree(node.Nodes, _searchtext);
                }
        }




















    }
}
