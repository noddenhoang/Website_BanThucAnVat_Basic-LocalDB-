<%@ Page Title="Tiệm ăn vặt" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TiemAnVat.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            function openSPDetail() {
                //alert("Opening modal!");
                //jQuery.noConflict();
                $('#modSPDetail').modal('show');
            }
            function openGiohang() {
                //alert("Opening modal!");
                //jQuery.noConflict();
                $('#ListGiohang').modal('show');
            }
            function validatePhoneNumber() {
                var input = document.getElementById("txtSoDienThoai").value;
                if (input.length > 10) {
                    alert("Số điện thoại chỉ được phép tối đa 10 số.");
                    document.getElementById("txtSoDienThoai").value = input.substring(0, 10);
                }
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Noidung" runat="server">
    <div class="container">
        <div class="col-sm-4">
            <asp:Label ID="lblMessage" runat="server" Text="" />
        </div>
        <div class="col-sm-12" style="text-align: right;">
            <asp:LinkButton ID="lbGiohang" runat="server" Font-Size="12px" OnClick="lbGiohang_Click" Text="Giỏ hàng"> </asp:LinkButton>
        </div>
        <div class="row"style="margin-top: 5px;">
        </div>
        <%-- Gridview --%>
        <div class="row" style="margin-top: 20px;">
            <div class="col-sm-12" style="text-align: center;">
                <asp:Label ID="LabelMenu" runat="server" Text="Thực đơn" CssClass="menu-label"></asp:Label>
                <asp:DropDownList ID="ListNhom" runat="server" DataSourceID="SqlDataSource1" Width="10%"
                    DataTextField="Tennhom" DataValueField="IDNhom" AutoPostBack="true" OnSelectedIndexChanged="ShowNhom" AppendDataBoundItems="true">
                   <asp:ListItem Text="-Tất cả-" Value="0" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"   
                ConnectionString="<%$ ConnectionStrings:TiemAnVatConnection %>"   
                SelectCommand="SELECT * FROM NhomSP"></asp:SqlDataSource>                
            </div>
            <div class="col-sm-12">
                <asp:GridView ID="listSanphams" runat="server" AutoGenerateColumns="False" AllowSorting="True" 
                    DataKeyNames="IdSP"
                    CssClass="table table-striped table-bordered table-condensed" BorderColor="Silver"
                    OnRowCommand="listSanphams_RowCommand"
                    EmptyDataText="">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="25px" />
                            <ItemStyle HorizontalAlign="Left" Font-Bold="true" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Hình ảnh">
                                <ItemTemplate>
                                    <asp:Image ID="IMG" runat="server" Width="100px" Height="100px" imageurl='<%#  "~/Images/"+Eval("img") %>'/>
                                </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="MaSP" HeaderText="Mã">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TenSP" HeaderText="Tên sản phẩm">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DVT" HeaderText="Đơn vị tính">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="GiaBan" HeaderText="Giá bán (VNĐ)" DataFormatString="{0:### ### ###}" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <%-- Update Company --%>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbChitietSanpham" runat="server" CommandArgument='<%# Eval("IdSP") %>'
                                    CommandName="XemSanpham" Text="Xem" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Modal to View a Sanpham Details-->
        <div class="modal fade" id="modSPDetail" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg" style="width: 600px;">
                <div class="modal-content" style="font-size: 11px;">

                    <div class="modal-header" style="text-align: center;">
                        <asp:Label ID="lblSanphamXem" runat="server" Text="Chi tiết sản phẩm" Font-Size="24px" Font-Bold="true" />
                    </div>

                    <div class="modal-body">
                        <div class="row">
                            <div class="col-sm-12">

                                <%-- Sanpham Details Textboxes --%>
                               <div class="col-sm-12">
                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-10">
                                            <asp:Image ID="Image1" runat="server" Width="500px" Height="500px"/>
                                        </div>
                                        <div class="col-sm-1"></div>
                                    </div>

                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-10">
                                            <label for="txtSanphamName" class="control-label">Tên sản phẩm:</label>
                                            <asp:TextBox ID="txtSanphamName" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                ToolTip="Tên sản phẩm"
                                                AutoCompleteType="Disabled" placeholder="Tên sản phẩm" ReadOnly="true" />
                                            <asp:Label runat="server" ID="lblSPID" Visible="false" Font-Size="12px" />
                                        </div>
                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-10">
                                            <label for="txtSanphamMa" class="control-label">Mã sản phẩm:</label>
                                            <asp:TextBox ID="txtSanphamMa" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                ToolTip="Mã sản phẩm"
                                                AutoCompleteType="Disabled" placeholder="Mã sản phẩm" ReadOnly="true" />
                                        </div>
                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-10">
                                            <label for="txtSanphamDVT" class="control-label">Đơn vị tính:</label>
                                            <asp:TextBox ID="txtSanphamDVT" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                ToolTip="Đơn vị tính"
                                                AutoCompleteType="Disabled" placeholder="Đơn vị tính" ReadOnly="true" />
                                        </div>
                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 20px;">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-10">
                                            <label for="txtDongia" class="control-label">Giá bán (VNĐ):</label>
                                            <asp:TextBox ID="txtDongia" runat="server" MaxLength="255" CssClass="form-control input-xs" 
                                                ToolTip="Giá bán (VNĐ)"
                                                AutoCompleteType="Disabled" placeholder="Giá bán sản phẩm" ReadOnly="true" />
                                        </div>
                                        <div class="col-sm-1">
                                        </div>
                                    </div>
                                   <div class="row" style="margin-top: 20px;">
                                    <div class="col-sm-1"></div>
                                    <div class="col-sm-10">
                                        <label for="txtSoLuong" class="control-label">Số lượng:</label>
                                        <asp:TextBox ID="txtSoLuong" runat="server" MaxLength="10" CssClass="form-control input-xs" 
                                            ToolTip="Số lượng sản phẩm" AutoCompleteType="Disabled" placeholder="Nhập số lượng" />
                                    </div>
                                    <div class="col-sm-1">
                                    </div>
                                </div>
                                </div>
                            </div>
                        </div>

                        <%-- Message label on modal page --%>
                        <div class="row" style="margin-top: 20px; margin-bottom: 10px;">
                            <div class="col-sm-1"></div>
                            <div class="col-sm-10">
                                <asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Size="12px" Text="" />
                            </div>
                            <div class="col-sm-1"></div>
                        </div>
                    </div>

                    <%-- Chon hang Buttons --%>
                    <div class="modal-footer">
                        <asp:Button ID="Button1" runat="server" class="btn btn-danger button-xs" data-dismiss="modal" 
                            Text="Bỏ vào giỏ"
                            Visible="true" CausesValidation="false"
                            OnClick="btnChonSanpham_Click"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="Button2" runat="server" class="btn btn-info button-xs" data-dismiss="modal" 
                            Text="Đóng" CausesValidation="false"
                            UseSubmitBehavior="false" />
                    </div>

                </div>
            </div>
        </div>

        <!-- Modal to View a List gio hang-->
        <div class="modal fade" id="ListGiohang" tabindex="-1" role="dialog" aria-labelledby="myGiohang" aria-hidden="true">
            <div class="modal-dialog modal-lg" style="width: 600px;">
                <div class="modal-content" style="font-size: 11px;">
                    <div class="modal-header" style="text-align: center;">
                        <asp:Label ID="Label2" runat="server" Text="Giỏ hàng" Font-Size="24px" Font-Bold="true" />
                    </div>
                    <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="True" 
                                DataKeyNames="IdSP"
                                CssClass="table table-striped table-bordered table-condensed" BorderColor="Silver"
                                OnRowDeleting="GridView1_RowDeleting"
                                EmptyDataText="Giỏ hàng trống">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                        <ItemStyle HorizontalAlign="Left" Font-Bold="true" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="TenSP" HeaderText="Tên sản phẩm">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Gia" HeaderText="Giá bán (VNĐ)" DataFormatString="{0:### ### ###}" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SoLuong" HeaderText="Số lượng" DataFormatString="{0:### ### ###}" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TongTien" HeaderText="Thành tiền" DataFormatString="{0:### ### ###}" >
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>

                                        <%-- Delete Sanpham --%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbXoaSanpham" Text="Xóa" runat="server"
                                                    OnClientClick="return confirm('Bạn chắc chắn muốn xóa sản phẩm này?');" CommandName="Delete" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                        </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <%-- Dat hang Buttons --%>
                    <div class="modal-footer">
                        <asp:Button ID="lbDathang" runat="server" class="btn btn-danger button-xs" data-dismiss="modal" 
                            Text="Đặt hàng"
                            Visible="true" CausesValidation="false"
                            OnClick="btnDathang_Click"
                            UseSubmitBehavior="false" />
                        <asp:Button ID="Button4" runat="server" class="btn btn-info button-xs" data-dismiss="modal" 
                            Text="Đóng" CausesValidation="false"
                            UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
        <!-- Modal để nhập thông tin khách hàng -->
            <div id="myModal" class="modal fade" role="dialog">
                <div class="modal-dialog">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Nhập Thông Tin Khách Hàng</h4>
                        </div>
                        <div class="modal-body">
                            <!-- Form nhập thông tin khách hàng -->
                            <div class="form-group">
                                <label for="txtTenKhachHang">Tên Khách Hàng:</label>
                                <asp:TextBox ID="txtTenKhachHang" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtSoDienThoai">Số Điện Thoại:</label>
                                <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" ClientIDMode="Static" oninput="validatePhoneNumber()"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtDiaChi">Địa Chỉ:</label>
                                <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtVC">Mã giảm giá (Nếu có):</label>
                                <asp:TextBox ID="txtVC" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnDatHangModal" runat="server" Text="Đặt Hàng" CssClass="btn btn-primary" OnClick="btnDatHangModal_Click" />
                        </div>
                    </div>
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
