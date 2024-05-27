<%@ Page Title="Tiệm ăn vặt | Đăng nhập" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TiemAnVat.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
        width: 133px;
    }
        .auto-style2 {
            text-align: center;
        }
        .giua {
            margin-left: auto;
            margin-right: auto;
        }
    .auto-style3 {
        text-align: center;
        height: 26px;
    }
    .auto-style4 {
        width: 351px;
        height: 26px;
    }
    .auto-style5 {
        height: 26px;
    }
    .auto-style6 {
        width: 351px;
        height: 28px;
    }
    .auto-style7 {
        height: 28px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Noidung" runat="server">

    <table class="giua" style="width: 300px; ">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style6">Tên đăng nhập</td>
            <td class="auto-style7">
                <asp:TextBox ID="TextBox1" runat="server" Width="146px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">Mật khẩu</td>
            <td class="auto-style5">
                <asp:TextBox ID="TextBox2" runat="server" TextMode="Password" Width="146px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style3" colspan="2">
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Đăng nhập" />
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Đăng ký" Width="92px" />
            </td>
        </tr>
        <tr>
            <td class="auto-style2" colspan="2">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
