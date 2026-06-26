Public Class frmIIAccount
    Inherits Sats.AppCore.frmSearch

    Private mv_strCaptionText As String
    Public Property CaptionText()
        Get
            Return mv_strCaptionText
        End Get
        Set(ByVal value)
            mv_strCaptionText = value
        End Set
    End Property
    Public Sub New(ByVal pv_strLanguage As String)
        MyBase.New(pv_strLanguage)
    End Sub

    Protected Overrides Function OnSearch(Optional ByVal pv_strIsLocal As String = "", Optional ByVal pv_strModule As String = "", Optional ByVal page As Integer = 1) As Integer
        MyBase.OnSearch()

        Dim v_strViewAccount, v_strDecode, v_strCMDSQL As String
        Dim v_intIndex, v_intMaxNo As Integer

        v_intMaxNo = 8
        v_strCMDSQL = MyBase.CMDSQL

        v_strViewAccount = "select iaacctno, iicode, sicode, micode"

        For v_intIndex = 1 To v_intMaxNo
            v_strViewAccount = v_strViewAccount & vbCrLf & ", typeno" & CStr(v_intIndex) & ", final_balance" & CStr(v_intIndex)
        Next
        v_strViewAccount = v_strViewAccount & vbCrLf & " from (select a.iaacctno, a.iicode, a.sicode, a.micode,"

        For v_intIndex = 1 To v_intMaxNo
            v_strViewAccount = v_strViewAccount & vbCrLf & "LEAD(a.typeno," & CStr(v_intIndex - 1) & ",0) OVER (PARTITION BY a.iicode ORDER BY typeno) typeno" & Format(v_intIndex, "0") & ","
            v_strViewAccount = v_strViewAccount & vbCrLf & "LEAD(a.final_balance," & CStr(v_intIndex - 1) & ",0) OVER (PARTITION BY a.iicode ORDER BY typeno) final_balance" & Format(v_intIndex, "0") & ","
        Next
        v_strViewAccount = v_strViewAccount & vbCrLf & "rank () over (partition by a.iicode order by typeno) rankno from (" _
                                                    & "select a.iaacctno, max (get_token(a.iaacctno,4,'.')) iicode," _
                                                    & " max (get_token(a.iaacctno,3,'.')) sicode," _
                                                    & " max (get_token(a.iaacctno,1,'.')) micode," _
                                                    & " max (get_token(a.iaacctno,2,'.')) typeno," _
                                                    & " max (a.balance)- sum (decode (nvl (b.operator, ''),'+', nvl (b.namt, 0),'-', -nvl (b.namt, 0),0)) first_balance," _
                                                    & " sum (decode (nvl (b.operator, ''),'+', nvl (b.namt, 0),0)) period_debit," _
                                                    & " sum (decode (nvl (b.operator, ''),'-', nvl (b.namt, 0),0)) period_credit," _
                                                    & " max (a.balance) - sum (decode (nvl (b.operator, ''),'+', nvl (b.namt, 0),'-', -nvl (b.namt, 0),0)) final_balance" _
                                                    & " from (select * from iamast where deleted = 0 and status = 0) a," _
                                                    & " (select * from iatran where field = 'balance' and deleted = 0 and status = 0 union all select * from tmp_iatrana where field = 'balance' and deleted = 0 and status = 0) b" _
                                                    & " where a.iaacctno = b.acctno(+) group by a.iaacctno) a)where rankno = 1 "

        v_strDecode = "select iaacctno, iicode, sicode, micode"
        For v_intIndex = 1 To v_intMaxNo
            v_strDecode = v_strDecode & vbCrLf & ", decode(to_number(typeno$),1, final_balance$, 0) final_balance" & v_intIndex
        Next

        v_strDecode = v_strDecode & vbCrLf & "from ( ?v_iaaccount )"

        For v_intIndex = 1 To v_intMaxNo
            Dim v_strTemp As String = Replace(v_strDecode, "$", v_intIndex)
            v_strCMDSQL = Replace(v_strCMDSQL, "?tabBalance" & v_intIndex, v_strTemp)
        Next

        v_strCMDSQL = Replace(v_strCMDSQL, "?v_iaaccount", v_strViewAccount)

        MyBase.CMDSQL = v_strCMDSQL
        Return MyBase.OnSearch(pv_strIsLocal, pv_strModule, page)
    End Function

End Class