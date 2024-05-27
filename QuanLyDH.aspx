<%@ Page Title="Tiệm ăn vặt | Quản lý đơn hàng" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="QuanLyDH.aspx.cs" Inherits="TiemAnVat.QuanLyDH" %>
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
                    <asp:Label ID="LabelMenu" runat="server" Text="Quản lý đơn hàng" CssClass="menu-label"></asp:Label>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"  
                    ConnectionString="<%$ ConnectionStrings:TiemAnVatConnection %>"   
                    SelectCommand="SELECT * FROM DonHang"></asp:SqlDataSource>                
                </div>
                <div class="col-sm-12">
                    <asp:GridView ID="gvDH" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                        DataKeyNames="Id"
                        CssClass="table table-striped table-bordered table-condensed" BorderColor="Silver"
                        OnRowDeleting="gvDH_RowDeleting"
                        OnRowCommand="listDH_RowCommand"
                        EmptyDataText="Không có dữ liệu">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                <ItemStyle HorizontalAlign="Left" Font-Bold="true" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Id" HeaderText="Mã đơn hàng">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TenDN" HeaderText="Tài khoản đặt hàng">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TenKhachHang" HeaderText="Tên khách hàng">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DiaChi" HeaderText="Địa chỉ">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SanPham" HeaderText="Thông tin sản phẩm">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MaGiamGia" HeaderText="Voucher">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TongTien" HeaderText="Tổng tiền (VNĐ)">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TongTienSauGiamGia" HeaderText="Tổng tiền cần thanh toán (VNĐ)">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                             <%-- Delete VC --%>
                             <asp:TemplateField>
                                 <ItemTemplate>
                                     <asp:LinkButton ID="lbDelDH" Text="Xoá" runat="server"
                                         OnClientClick="return confirm('Bạn chắc chắn muốn xóa đơn đặt hàng này?');" CommandName="Delete" />
                                 </ItemTemplate>
                                 <HeaderStyle HorizontalAlign="Left" />
                                 <ItemStyle HorizontalAlign="Center" Width="50px" />
                             </asp:TemplateField>
                            <%-- Update Company --%>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbChitietDH" runat="server" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="XemDH" Text="Xem" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="80px" />
                                </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <!-- Modal -->
                <div id="hoaDonModal" class="modal fade" role="dialog">
                  <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                      <div class="modal-header">
                        <h4 class="modal-title">Thông tin đặt hàng</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                      </div>
                      <div class="modal-body">
                        <asp:Label ID="lblHoaDon" runat="server" Text=""></asp:Label>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                      </div>
                    </div>
                  </div>
                </div>
        </div>
</asp:Content>