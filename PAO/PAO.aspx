<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/oceans.Master" CodeBehind="PAO.aspx.vb" Inherits="oceansV2.PAO" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server"  TargetControlID="heading" Radius="10" />
    <div id="heading" runat="server" align="center" style="background-color:darkorchid;">
       <p style="color :white ">PAO Detail</p> 
    </div>
    <div id="entry" align="center" >

        <table id="tblsanction" border="1" runat="server">
           
            <tr>


                <td align="left">

                    <asp:Label ID="lblreldatefrom" runat="server" Text="Release Date From"></asp:Label></td>
                <td style="width: 100px" align="left">
                    <asp:TextBox ID="txtfromdate" runat="server"></asp:TextBox>

                    <asp:RegularExpressionValidator ID="valefromdate" runat="server" ControlToValidate="txtfromdate"
                        Display="Dynamic" ErrorMessage="Input Proper Date(dd/mm/yyyy)" ValidationExpression="^([0-3]\d)/([01]\d)/(\d{4})$"></asp:RegularExpressionValidator>
                    <cc1:calendarextender id="CalendarExtender1" runat="server" format="dd/MM/yyyy" targetcontrolid="txtfromdate" popupposition="TopLeft">
                    </cc1:calendarextender>
                    <asp:CompareValidator ID="valcfromdate" Display="Dynamic" runat="server" Type="Date" Operator="LessThanEqual"  ControlToValidate="txtfromdate" ErrorMessage="Date Should't Be Greater Than Today's Date"></asp:CompareValidator>

                    <%--  <asp:TextBox ID="TxtFinYear" runat="server" ReadOnly="true" Enabled="False"></asp:TextBox>--%>

                    <%-- <asp:RequiredFieldValidator ID="valryear" runat="server" ErrorMessage="*" ControlToValidate="TxtFinYear" Display="Dynamic" InitialValue="Select Year" >*</asp:RequiredFieldValidator>--%>

                    
                </td>
            </tr>




            <tr>
                <td>
                    <asp:Label ID="lblscheme" runat="server" Text="Select Scheme"></asp:Label></td>
                <td align="left">
                    <asp:DropDownList ID="ddlschemecode" runat="server" AutoPostBack="True">
                       
                    </asp:DropDownList>
                    
                </td>
            </tr>

            <tr>

                <td colspan="2">

                    <asp:Button ID="btngenerate" runat="server" Text="Proceed" CssClass="btn btn-info" />
                </td>

            </tr>
            <tr>

                <td colspan="2">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
            </tr>
        </table>
        
        <div style="margin-bottom: 10px">
        </div>

        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False"
            OnSelectedIndexChanged="GridView4_SelectedIndexChanged" Visible="False" AllowPaging="true">
            <pagersettings mode="NumericFirstLast"
          position="Bottom"           
          pagebuttoncount="8" />
            
                      
        <pagerstyle backcolor="LightBlue"
          height="30px"
          verticalalign="Bottom"
          horizontalalign="Center"/>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnSelect" runat="server" CausesValidation="true" CommandName="Select"
                            Text="Select" CommandArgument='<%# Eval("RecomId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RecomID" HeaderText="RecomId" />
                <asp:BoundField DataField="YearCode" HeaderText="YearCode" />


                <asp:TemplateField HeaderText="Category" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblcategory" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RecomDate" HeaderText="Release Date" DataFormatString="{0:dd/MM/yy}" />
                <asp:BoundField DataField="RecomAmount" Visible="False" HeaderText="RecomAmount" />
                <asp:BoundField DataField="RecomLoanAmt" Visible="False" HeaderText="RecomLoanAmt" />
                <asp:BoundField DataField="RecomGrantAmt" Visible="False" HeaderText="RecomGrantAmt" />
                <asp:BoundField DataField="ReleaseAmount" HeaderText="ReleaseAmount" />
                <asp:BoundField DataField="ReleaseLoanAmount" Visible="False" HeaderText="ReleaseLoanAmount" />
                <asp:BoundField DataField="ReleaseGrantAmount" Visible="False" HeaderText="ReleaseGrantAmount" />
                <asp:BoundField DataField="SanctionNo" HeaderText="SanctionNo" />
                <asp:BoundField DataField="RecomStatus" Visible="False" HeaderText="RecomStatus" />
                <asp:BoundField DataField="RecomDocument" Visible="False" HeaderText="RecomDocument" />
                <asp:BoundField DataField="RecomFresh" Visible="False" HeaderText="RecomFresh" />
                <asp:BoundField DataField="RecomYesNo" Visible="False" HeaderText="RecomYesNo" />
                <asp:BoundField DataField="RecomForwardTo" Visible="False" HeaderText="RecomForwardTo" />
                <asp:BoundField DataField="PAOApproved" Visible="False" HeaderText="PAOApproved" />
                <asp:BoundField DataField="SchemeName" HeaderText="SchemeName" />
                <asp:BoundField DataField="IGANo" HeaderText="IGA No" Visible="False" />
                <asp:BoundField DataField="PAOApprovedDate" HeaderText="PAO Approved Date" Visible="False" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkbtnAccept4" runat="server"
                            Text="Enter Data" CausesValidation="false" CommandName="Accept" CommandArgument='<%# Eval("RecomId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkbtnReject4" runat="server"
                            Text="Reject" CausesValidation="false" CommandName="Reject" CommandArgument='<%# Eval("RecomId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:GridView ID="GrdReleaseDetail" runat="server" AutoGenerateColumns="False"
            Visible="False" AllowPaging="true" PageSize="10">
            <Columns>
                <asp:BoundField DataField="StateName" HeaderText="StateName" />
                <asp:BoundField DataField="RecomDetailLoanAmount" HeaderText="RecomDetailLoanAmount" />
                <asp:BoundField DataField="RecomDetailGrantAmount" HeaderText="RecomDetailGrantAmount" />
                <asp:BoundField DataField="ReleaseDetailLoanAmount" HeaderText="ReleaseDetailLoanAmount" />
                <asp:BoundField DataField="ReleaseDetailGrantAmount" HeaderText="ReleaseDetailGrantAmount" />
                <asp:BoundField DataField="Granthead" HeaderText="Grant head" />
                <asp:BoundField DataField="Loanhead" HeaderText="Loan head" />
            </Columns>
        </asp:GridView>


        <div>
            <asp:Literal ID="litmsg" runat="server" EnableViewState="false" Visible="false"></asp:Literal>
        </div>
        <div>
            <asp:Button runat="server" ID="btnback" Text="Back to Previous Data." Visible="false" />
        </div>

        <asp:Label runat="server" Visible="false" Text="" ID="lblaccept"></asp:Label>
        <div runat="server" id="divAccept" align="center" width="80px" height="100px" visible="false" style="border: 1px">

            <table border="2">
                <tr>
                    <td>IGA No  
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtIGA" EnableViewState="false"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left" style="width: 100px">
                        <asp:Label ID="lblReleaseDate" runat="server" Text="Release Date From PAO"></asp:Label></td>
                    <td style="width: 100px" align="left">
                        <asp:TextBox ID="txtPAOfromdate" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPAOfromdate"
                            Display="Dynamic" ErrorMessage="Input Proper Date(dd/mm/yyyy)" ValidationExpression="^([0-3]\d)/([01]\d)/(\d{4})$"></asp:RegularExpressionValidator>
                        <cc1:CalendarExtender id="CalendarExtender2" runat="server" format="dd/MM/yyyy" targetcontrolid="txtPAOfromdate" popupposition="TopLeft"></cc1:CalendarExtender>

                    </td>
                </tr>
                <tr>
                    <td style="margin-left: 20px; height: 17px;">
                        <asp:Button ID="btnSavePAO" runat="server" Text="Save" Style="height: 26px" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancelPAO" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>

        <div runat="server" id="divRemark" align="center" style="width: 160px;" visible="false">
            <table border="3">
                <tr>
                    <td>Reason  
                    </td>
                    <td>
                        <asp:TextBox ID="txtReject" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="margin-left: 20px; height: 17px;">
                        <asp:Button ID="BtnSave" runat="server" Text="Submitt" Style="height: 26px" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>





        <br />


    </div>
</asp:Content>
