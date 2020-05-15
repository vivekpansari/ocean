Imports System.IO
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Net
Imports System.Net.Mail
Public Class PAO
    Inherits BaseClass
    Dim releaseamt As Double
    Dim SchemeType As String
    Dim FCDYes As Boolean
    Dim FCDVal As String
    Dim dt As DataTable
    Dim releasedate As DateTime
    Dim releasedate1 As DateTime
    Dim diff As Integer
    Dim todaydate As DateTime
    Dim sanctionid As Integer
    Dim iNumberOfDays As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MyBase.LogActivity("oceans", True)
        lblmsg.Text = ""
        Try
            valcfromdate.ValueToCompare = System.DateTime.Now.ToShortDateString
            If Not Page.IsPostBack Then

                If User.IsInRole("PAO") Then
                    Session("Role") = "PAO"
                    SchemeType = "SPS"
                    FCDYes = False
                End If
                Session("FCDYes") = FCDYes
                Session("YearCode") = "2018-2019"



                ddlschemecode.DataSource = DAL.GetDataBySchemeTypeYearCode("SPS", "2018-2019")
                ddlschemecode.DataTextField = "SchemeName"
                ddlschemecode.DataValueField = "SchemeCode"
                ddlschemecode.DataBind()
                ddlschemecode.Items.Insert(0, "Select Scheme")

                'End Code
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btngenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btngenerate.Click
        GridView4.Visible = True

        If ddlschemecode.SelectedIndex <> 0 And txtfromdate.Text <> "" Then
            dt = DAL.BindPAOGridDatewise(1, ddlschemecode.SelectedValue.ToString(), txtfromdate.Text)
        ElseIf ddlschemecode.SelectedIndex <> 0 And txtfromdate.Text = "" Then

            dt = DAL.BindPAOGridDatewise(2, ddlschemecode.SelectedValue.ToString(), DateTime.Now)
        ElseIf ddlschemecode.SelectedIndex = 0 And txtfromdate.Text <> "" Then
            dt = DAL.BindPAOGridDatewise(3, "", txtfromdate.Text)
        End If


        GridView4.DataSource = dt
        GridView4.DataBind()
        If GridView4.Rows.Count > 0 Then
            GridView4.Visible = True
            tblsanction.Visible = True


        Else
            lblmsg.Text = "No Release Available For This Scheme"

        End If

    End Sub

    Protected Sub bindGridView4()
        If ddlschemecode.SelectedIndex <> 0 And txtfromdate.Text <> "" Then
            dt = DAL.BindPAOGridDatewise(1, ddlschemecode.SelectedValue.ToString(), txtfromdate.Text)
        ElseIf ddlschemecode.SelectedIndex <> 0 And txtfromdate.Text = "" Then

            dt = DAL.BindPAOGridDatewise(2, ddlschemecode.SelectedValue.ToString(), DateTime.Now)
        ElseIf ddlschemecode.SelectedIndex = 0 And txtfromdate.Text <> "" Then
            dt = DAL.BindPAOGridDatewise(3, "", txtfromdate.Text)
        End If

        GridView4.DataSource = dt
        GridView4.DataBind()

    End Sub
    Protected Sub GridView4_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView4.SelectedIndexChanged
        ViewState("RecomId") = GridView4.Rows(GridView4.SelectedIndex).Cells(1).Text
        dt = DAL.BindPAOReleaseDetail(ViewState("RecomId"))
        GrdReleaseDetail.DataSource = dt
        GrdReleaseDetail.DataBind()
        GridView4.Visible = False
        GrdReleaseDetail.Visible = True

        btnback.Visible = True
    End Sub
    Protected Sub btnback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnback.Click
        If ddlschemecode.SelectedValue.ToString() = "Select Scheme" Then
            dt = DAL.BindPAOGridwithdate(Session("YearCode"), txtfromdate.Text)
        Else
            dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
        End If
        lblaccept.Visible = False
        GridView4.DataSource = dt
        GridView4.DataBind()
        GridView4.Visible = True
        GrdReleaseDetail.Visible = False
        btnback.Visible = False

    End Sub

    Protected Sub GridView4_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView4.RowCommand
        Try

            Dim LnkbtnAccept As LinkButton = GridView4.FindControl("LnkbtnAccept4")
            Dim LnkbtnReject As LinkButton = GridView4.FindControl("LnkbtnReject4")
            Dim PAOStatus As DataTable
            Dim IGANo As String
            Dim PAOApprovedDate As String
            Dim IGAdt As DataTable

            ViewState("RecomId") = Convert.ToInt32(e.CommandArgument)
            If ddlschemecode.SelectedValue.ToString() = "" Then
                dt = DAL.BindPAOGridbyrecomid(Session("YearCode"), ViewState("RecomId"))
            Else

                dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
            End If

            releasedate = dt.Rows(0).Item("RecomDate")

            If dt.Rows.Count > 0 Then
                If e.CommandName = "Accept" Then
                    IGAdt = DAL.FetchIGAData(ViewState("RecomId"))
                    IGANo = IGAdt.Rows(0).Item("IGANo").ToString()
                    PAOApprovedDate = IGAdt.Rows(0).Item("PAOApprovedDate").ToString()

                    If IGANo <> "" And PAOApprovedDate <> "" Then
                        lblaccept.Text = "Record Already Entered."
                        lblaccept.ForeColor = System.Drawing.Color.Green
                        lblaccept.Font.Bold = True
                        lblaccept.Font.Size = 18
                        lblaccept.Visible = True
                        GridView4.Visible = False
                        btnback.Visible = True
                    Else
                        divAccept.Visible = True
                        GridView4.Visible = False
                    End If

                ElseIf e.CommandName = "Reject" Then
                    PAOStatus = DAL.InsertPAOStatus(ViewState("RecomId").ToString(), "R", System.DateTime.Now(), "N")
                    dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    txtReject.Text = ""
                    divRemark.Visible = True
                    GridView4.Visible = False
                    divAccept.Visible = False

                End If
            End If


            'iNumberOfDays = DetermineNumberofDays(releasedate)
            'DAL.InsertDateDifference(ViewState("RecomId"), iNumberOfDays)

        Catch ex As Exception

        End Try

    End Sub

    Function DetermineNumberofDays(ByVal releasedate As DateTime) As Integer
        Dim tsTimeSpan As TimeSpan
        tsTimeSpan = Now.Subtract(releasedate)
        iNumberOfDays = tsTimeSpan.Days
        Return iNumberOfDays
    End Function

    Protected Sub GridView4_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView4.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            ViewState("SchemeName") = DataBinder.Eval(e.Row.DataItem, "SchemeName")

            'lbSchemeName.Text = ViewState("SchemeName").ToString()
            If (e.Row.DataItem("PAOApproved").ToString = "R") Then
                e.Row.BackColor = Drawing.Color.DeepSkyBlue
            End If

            If (e.Row.DataItem("PAOApproved").ToString = "O") Then
                e.Row.BackColor = Drawing.Color.LightSkyBlue
            End If

        End If

        txtIGA.Text = ""
        txtPAOfromdate.Text = ""

    End Sub



    Protected Sub GrdReleaseDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GrdReleaseDetail.PageIndexChanging
        GrdReleaseDetail.PageIndex = e.NewPageIndex
        ViewState("RecomId") = GridView4.Rows(GridView4.SelectedIndex).Cells(1).Text
        dt = DAL.BindPAOReleaseDetail(ViewState("RecomId"))
        GrdReleaseDetail.DataSource = dt
        GrdReleaseDetail.DataBind()
        GridView4.Visible = False
        GrdReleaseDetail.Visible = True
        btnback.Visible = True
    End Sub


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        'Insert the reason of cancellation
        Try
            DAL.InsertRemark(ViewState("RecomId"), txtReject.Text)
            divRemark.Visible = False

            dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
            If dt.Rows.Count > 0 Then
                GridView4.DataSource = dt
                GridView4.DataBind()
                GridView4.Visible = True
            Else
                lblaccept.Text = "No Sanction available."
                lblaccept.Visible = True

            End If

        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Redirect  to grid
        Try
            DAL.InsertPAOStatus(ViewState("RecomId").ToString(), "F", System.DateTime.Now(), "Y")

            'dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
            If ddlschemecode.SelectedValue.ToString() = "Select Scheme" Then
                dt = DAL.BindPAOGridwithdate(Session("YearCode"), txtfromdate.Text)
            Else
                dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
            End If
            If dt.Rows.Count > 0 Then
                GridView4.DataSource = dt
                GridView4.DataBind()
                GridView4.Visible = True
                divRemark.Visible = False

            Else
                lblaccept.Text = "No Sanction available."
                lblaccept.Visible = True
                divRemark.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnSavePAO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavePAO.Click
        'Insert 
        Try

            DAL.InsertPAOAcceptedStatus(ViewState("RecomId"), txtIGA.Text, Convert.ToDateTime(txtPAOfromdate.Text))
            lblaccept.Text = "Data Saved Successfully."

            lblaccept.Visible = True
            divRemark.Visible = False


            divAccept.Visible = False

        Catch ex As Exception
            lblaccept.Text = "ERROR: Data could not be Saved."
        End Try
    End Sub

    Protected Sub btnCancelPAO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelPAO.Click
        'Redirect  to grid
        Try
            DAL.InsertPAOStatus(ViewState("RecomId").ToString(), "F", System.DateTime.Now(), "Y")

            If ddlschemecode.SelectedValue.ToString() = "Select Scheme" Then
                dt = DAL.BindPAOGridwithdate(Session("YearCode"), txtfromdate.Text)
            Else
                dt = DAL.BindPAOGrid(Session("YearCode"), ddlschemecode.SelectedValue.ToString())
            End If

            If dt.Rows.Count > 0 Then
                GridView4.DataSource = dt
                GridView4.DataBind()
                GridView4.Visible = True
                divAccept.Visible = False

            Else
                lblmsg.Text = "No Sanction available."
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GridView4_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView4.PageIndexChanging
        GridView4.PageIndex = e.NewPageIndex
        bindGridView4()
    End Sub
End Class