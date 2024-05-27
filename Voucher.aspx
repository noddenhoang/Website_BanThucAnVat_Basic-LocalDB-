<%@ Page Title="Tiệm ăn vặt | Săn Voucher" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Voucher.aspx.cs" Inherits="TiemAnVat.Voucher" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Noidung" runat="server">
        <div class="container">
            <div class="col-sm-4">
                <asp:Label ID="lblMessage" runat="server" Text="" />
            </div>
            <div class="col-sm-12" style="text-align: right;">
            </div>
            <%-- Gridview --%>
            <div class="row" style="margin-top: 20px;">
                <div class="col-sm-12" style="text-align: center;">
                    <asp:Label ID="LabelMenu" runat="server" Text="Danh sách Voucher" CssClass="menu-label"></asp:Label>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"  
                    ConnectionString="<%$ ConnectionStrings:TiemAnVatConnection %>"   
                    SelectCommand="SELECT * FROM MaGiamGia"></asp:SqlDataSource>                
                </div>
                <div class="col-sm-12">
                    <asp:GridView ID="gvMaGG" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                        DataKeyNames="IdVC"
                        CssClass="table table-striped table-bordered table-condensed" BorderColor="Silver"
                        EmptyDataText="Không có dữ liệu">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                <ItemStyle HorizontalAlign="Left" Font-Bold="true" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="MaVoucher" HeaderText="Mã giảm giá">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PhanTramGiam" HeaderText="Phần trăm giảm giá">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
</asp:Content>