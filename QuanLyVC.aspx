<%@ Page Title="Tiệm ăn vặt | Quản lý Voucher" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="QuanLyVC.aspx.cs" Inherits="TiemAnVat.QuanLyVC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            function openVCDetail() {
                //alert("Opening modal!");
                //jQuery.noConflict();
                $('#modVCDetail').modal('show');
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Noidung" runat="server">
        <div class="container">
            <div class="col-sm-4">
                <asp:Label ID="lblMessage" runat="server" Text="" />
            </div>
            <div class="col-sm-12" style="text-align: right;">
                <asp:LinkButton ID="lbNewVC" runat="server" Font-Size="12px" OnClick="lbNewVC_Click">Thêm Voucher</asp:LinkButton>
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
                        OnRowDeleting="gvMaGG_RowDeleting"
                        OnRowCommand="gvMaGG_RowCommand"
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
                            <asp:BoundField DataField="PhanTramGiam" HeaderText="Phần trăm giảm giá" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <%-- Delete VC --%>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbDelVC" Text="Xoá" runat="server"
                                        OnClientClick="return confirm('Bạn chắc chắn muốn xóa Voucher này?');" CommandName="Delete" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>

                            <%-- Update Company --%>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbUpdVC" runat="server" CommandArgument='<%# Eval("IdVC") %>'
                                        CommandName="UpdVC" Text="Cập nhật" CausesValidation="false"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Modal to Add New or View / Update a Sanpham Details-->
        <div class="modal fade" id="modVCDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg" style="width: 600px;">
                <div class="modal-content" style="font-size: 11px;">

                    <div class="modal-header" style="text-align: center;">
                        <asp:Label ID="lblVCNew" runat="server" Text="Thêm Voucher mới" Font-Size="24px" Font-Bold="true" />
                        <asp:Label ID="lblVCUpd" runat="server" Text="Xem / Cập nhật Voucher" Font-Size="24px" Font-Bold="true" />
                    </div>

                    <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <%-- Sanpham Details Textboxes --%>
                                    <div class="col-sm-12">
                                        <div class="row" style="margin-top: 20px;">
                                            <div class="col-sm-1"></div>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtMaVC" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                    ToolTip="Mã Voucher"
                                                    AutoCompleteType="Disabled" placeholder="Mã Voucher" />
                                                <asp:Label runat="server" ID="lblVCID" Visible="false" Font-Size="12px" />
                                            </div>
                                            <div class="col-sm-1">
                                            </div>
                                        </div>
                                        <div class="row" style="margin-top: 20px;">
                                            <div class="col-sm-1"></div>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtPhanTramGiam" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                    ToolTip="Phần trăm giảm giá"
                                                    AutoCompleteType="Disabled" placeholder="Phần trăm giảm giá" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                        <%-- Message label on modal page --%>
                        <div class="row" style="margin-top: 20px; margin-bottom: 10px;">
                            <div class="col-sm-1"></div>
                            <div class="col-sm-10">
                                <asp:Label ID="lblModalMessage" runat="server" ForeColor="Red" Font-Size="12px" Text="" />
                            </div>
                            <div class="col-sm-1"></div>
                        </div>
                    </div>

                    <%-- Add, Update and Cancel Buttons --%>
                    <div class="modal-footer">
                        <asp:Button ID="btnAddVC" runat="server" class="btn btn-danger button-xs" data-dismiss="modal" 
                            Text="Thêm Voucher"
                            Visible="true" CausesValidation="false"
                            OnClick="btnAddVC_Click"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="btnUpdVC" runat="server" class="btn btn-danger button-xs" data-dismiss="modal" 
                            Text="Cập nhật Voucher"
                            Visible="false" CausesValidation="false"
                            OnClick="btnUpdVC_Click"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="btnClose" runat="server" class="btn btn-info button-xs" data-dismiss="modal" 
                            Text="Đóng" CausesValidation="false"
                            UseSubmitBehavior="false" />
                    </div>

                </div>
            </div>
</asp:Content>