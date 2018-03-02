<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageEdit.aspx.cs" Inherits="Mozart.WebService.ImageEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <link href="style/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <script src="script/jquery.min.js" type="text/javascript"></script>
    <script src="script/jquery.Jcrop.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(function ($) {

            // Create variables (in this scope) to hold the API and image size
            var jcrop_api, boundx, boundy;

            $('#target').Jcrop({
                onChange: updatePreview,
                onSelect: updatePreview,
                aspectRatio: 1,
                maxSize: [120, 120],
                setSelect: [0, 0, 120, 120]
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];
                // Store the API in the jcrop_api variable
                jcrop_api = this;
            });

            function updatePreview(c) {

                if (parseInt(c.w) > 0) {
                    var rx = 120 / c.w;
                    var ry = 120 / c.h;

                    $('#preview').css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }
                $('#x1').val(c.x);
                $('#y1').val(c.y);
                $('#x2').val(c.x2);
                $('#y2').val(c.y2);
                $('#Iwidth').val(c.w);
                $('#Iheight').val(c.h);

            };

        });

  </script>
</head>
<body>
        <span>原始图片</span>
      <asp:Image ID="target" runat="server" />
      <div style="width:100px;height:100px;overflow:hidden;">
      <span>最终显示效果</span>
      <div style="width:120px;height:120px;overflow:hidden;">
		<asp:Image ID="preview" alt="Preview" runat="server" />
        </div>
	</div>
    <form id="form1" runat="server">
    <div>
            <asp:TextBox ID="x1" runat="server"></asp:TextBox>
        <asp:TextBox ID="y1" runat="server"></asp:TextBox>
        <asp:TextBox ID="x2" runat="server"></asp:TextBox>
        <asp:TextBox ID="y2" runat="server"></asp:TextBox>
        <asp:TextBox ID="Iwidth" runat="server"></asp:TextBox>
        <asp:TextBox ID="Iheight" runat="server"></asp:TextBox>
        <br/>
        <asp:Button ID="Button1" runat="server" Text="裁剪并保存图片" OnClick="saveImg" />
    </div>
    </form>
</body>
</html>
