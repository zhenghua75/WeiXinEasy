using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.SYS;
using Model.SYS;
using Mozart.Common;

namespace Mozart.CMSAdmin.SysAdmin
{
    public partial class wfmSYSMenuIndustry : System.Web.UI.Page
    {
        SYSMenuIndustryDAL dal = new SYSMenuIndustryDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               //获取行业菜单
                SysCategoryDAL categorymenudal = new SysCategoryDAL();
                DataSet dsRole = categorymenudal.GetNoDelSysCateGoryLsit("");
                DataRow drRole = dsRole.Tables[0].NewRow();
                drRole["ID"] = "";
                drRole["SiteName"] = "--请选择行业类别--";
                dsRole.Tables[0].Rows.InsertAt(drRole, 0);
                ddlCategory.DataSource = dsRole.Tables[0].DefaultView;
                ddlCategory.DataTextField = "SiteName";
                ddlCategory.DataValueField = "ID";
                ddlCategory.DataBind();
                BindTreeView("");
            }
        }

        protected void BindTreeView(string strMenu)
        {
            tvModel.Nodes.Clear();
            DataSet ds = dal.GetSYSMenuIndustryByWhere(" a.[Parent] = '' ");
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
            DataSet ds = dal.GetSYSMenuIndustryByWhere(" a.[Parent] = '" + nodeid + "' ");
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
            string steMenu = dal.GetSYSMenuByCategory(ddlCategory.SelectedItem.Value.ToString());
            BindTreeView(steMenu);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedItem.Value.ToString() == "" || ddlCategory.SelectedItem.Value.ToString() == null)
            {
                MessageBox.Show(this, "操作失败！请选择一个类别"); return;
            }
            if (DispSelectTreeNodes().Length == 0)
            {
                MessageBox.Show(this, "操作失败！请保证必须选一个"); return;
            }
            if (dal.UpdateSYSMenuIndustry(ddlCategory.SelectedItem.Value.ToString(), DispSelectTreeNodes()) == 0)
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