using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.CMSAdmin.SysAdmin
{
    public partial class wfmRightManage : System.Web.UI.Page
    {
        DAL.SYS.MenuRoleDAL dal = new DAL.SYS.MenuRoleDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //角色
                DAL.SYS.MenuRoleDAL dalRole = new DAL.SYS.MenuRoleDAL();
                DataSet dsRole = null;
                switch (GlobalSession.strRoleCode)
                {
                    case "ADMIN":
                        dsRole = dalRole.GetRoleList();
                        break;
                    case "AGENT":
                        DataRow drRole = dsRole.Tables[0].NewRow();
                        drRole["No"] = "PTKH";
                        drRole["Name"] = "普通客户";
                        dsRole.Tables[0].Rows.InsertAt(drRole, 0);
                        break;
                    default:
                        break;
                }
                this.ddlRole.DataSource = dsRole.Tables[0].DefaultView;
                this.ddlRole.DataTextField = "Name";
                this.ddlRole.DataValueField = "No";
                this.ddlRole.DataBind();

                BindTreeView("");
            }
        }

        protected void BindTreeView(string strMenu)
        {
            tvModel.Nodes.Clear();
            DataSet ds = dal.GetMenuListByWhere(" a.[Parent] = '' ");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Model.SYS.SYS_Menu model = DataConvert.DataRowToModel<Model.SYS.SYS_Menu>(row);
                string name = model.Name;
                string id = model.No;
                TreeNode td = new TreeNode(name, id);
                if (strMenu.Contains(id))
                {
                    td.Checked = true;
                }
                td.SelectAction = TreeNodeSelectAction.None;
                BindChildTree(td, strMenu);
                tvModel.Nodes.Add(td);
            }
            tvModel.ExpandAll();
        }

        protected void BindChildTree(TreeNode node, string strMenu)
        {
            string nodeid = node.Value;
            DataSet ds = dal.GetMenuListByWhere(" a.[Parent] = '" + nodeid + "' ");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Model.SYS.SYS_Menu model = DataConvert.DataRowToModel<Model.SYS.SYS_Menu>(row);
                string name = model.Name;
                string id = model.No;
                TreeNode td = new TreeNode(name, id);
                if (strMenu.Contains(id))
                {
                    td.Checked = true;
                }
                td.SelectAction = TreeNodeSelectAction.None;
                BindChildTree(td, strMenu);
                node.ChildNodes.Add(td);
            }
        }

        public string DispSelectTreeNodes()//显示选中的树节点编号
        {
            string strDisp = "";
            foreach (TreeNode node in this.tvModel.Nodes)
            {
                if (node.Checked)
                {
                    strDisp += node.Value + ",";
                }
                strDisp += GetCheckedNodes(node);
            }
            strDisp = strDisp.Substring(0, strDisp.Length - 1);
            return strDisp;
        }

        private string GetCheckedNodes(TreeNode parentNode)
        {
            string strReutrn = "";
            foreach (TreeNode node in parentNode.ChildNodes)
            {
                if (node.Checked == true)
                {
                    strReutrn += node.Value.ToString() + ",";
                }
                if (node.ChildNodes.Count > 0)
                {
                    strReutrn += GetCheckedNodes(node);
                }
            }
            return strReutrn;

        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            string steMenu = dal.GetMenuByRole(this.ddlRole.SelectedItem.Value.ToString());
            BindTreeView(steMenu);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //this.lbNodes.Text = this.DispSelectTreeNodes();
            if (this.ddlRole.SelectedItem.Value.ToString() == "ADMIN")
            {
                return;
            }
            if (dal.UpdateRoleMenu(this.ddlRole.SelectedItem.Value.ToString(), this.DispSelectTreeNodes()) == 0)
            {
                MessageBox.Show(this, "权限修改失败！");
            }
            else
            {
                MessageBox.Show(this, "权限修改成功！");
            }
        }
    }
}