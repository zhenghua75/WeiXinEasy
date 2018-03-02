using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.SYS;
using DAL.SYS;
using Mozart.Common;

namespace Mozart.CMSAdmin.SysAdmin
{
    public partial class wfmSYSMenuSiteCode : System.Web.UI.Page
    {
        SYSMenuSiteCodeDAL dal = new SYSMenuSiteCodeDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //获取行业菜单
                AccountDAL accountdal = new AccountDAL();
                DataSet dsRole = accountdal.GetAccountList("");
                DataRow drRole = dsRole.Tables[0].NewRow();
                drRole["SiteCode"] = "";
                drRole["LoginName"] = "--请选择用户--";
                dsRole.Tables[0].Rows.InsertAt(drRole, 0);
                ddlSiteCode.DataSource = dsRole.Tables[0].DefaultView;
                ddlSiteCode.DataTextField = "LoginName";
                ddlSiteCode.DataValueField = "SiteCode";
                ddlSiteCode.DataBind();
                BindTreeView("");
            }
        }

        protected void BindTreeView(string strMenu)
        {
            tvModel.Nodes.Clear();
            DataSet ds = dal.GetSYSMenuSiteCodeByWhere(" a.[Parent] = '' ");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SYS_Menu model = DataConvert.DataRowToModel<SYS_Menu>(row);
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
            DataSet ds = dal.GetSYSMenuSiteCodeByWhere(" a.[Parent] = '" + nodeid + "' ");
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
            string steMenu = dal.GetStringSYSMenuSiteCode(ddlSiteCode.SelectedItem.Value.ToString());
            BindTreeView(steMenu);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlSiteCode.SelectedItem.Value.ToString() == "" || ddlSiteCode.SelectedItem.Value.ToString() == null)
            {
                MessageBox.Show(this, "操作失败！请选择一个用户"); return;
            }
            if (DispSelectTreeNodes().Length == 0)
            {
                MessageBox.Show(this, "操作失败！请保证必须选一个"); return;
            }
            if (dal.UpdateSYSMenuSiteCode(ddlSiteCode.SelectedItem.Value.ToString(), DispSelectTreeNodes()) == 0)
            {
                MessageBox.Show(this, "操作失败！");
            }
            else
            {
                MessageBox.Show(this, "操作成功！");
            }
        }
    }
}