Imports Sats.CommonLibrary
'Imports System.EnterpriseServices

'<JustInTimeActivation(False), _
'Transaction(TransactionOption.Disabled), _
'ObjectPooling(Enabled:=True, MinPoolSize:=30)> _
Public Class objRouter
    Implements IDisposable
    'Inherits ServicedComponent

    Public Function Transfer(ByRef pv_strObjMessage As String) As Long
        Dim v_lngErrorCode As Long
        Dim v_xmlDocument As New XmlDocumentEx

        Try
            'Read object message 
            v_xmlDocument.LoadXml(pv_strObjMessage)

            Dim v_attrColl As Xml.XmlAttributeCollection = v_xmlDocument.DocumentElement.Attributes
            Dim v_strObjectName As String
            Dim v_strActionFlag As String
            Dim v_strCmdInquiry As String

            If Not (v_attrColl.GetNamedItem(gc_AtributeOBJNAME) Is Nothing) Then
                v_strObjectName = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeOBJNAME), Xml.XmlAttribute).Value)
            Else
                v_strObjectName = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeACTFLAG) Is Nothing) Then
                v_strActionFlag = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeACTFLAG), Xml.XmlAttribute).Value)
            Else
                v_strActionFlag = String.Empty
            End If
            If Not (v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY) Is Nothing) Then
                v_strCmdInquiry = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeCMDINQUIRY), Xml.XmlAttribute).Value)
            Else
                v_strCmdInquiry = String.Empty
            End If

            If Trim(v_strObjectName) = OBJNAME_SY_AUTHENTICATION Then
                Dim v_objSystemAdmin As New SystemAdmin
                'Xử lý đặc biệt
                Dim v_strFuncName As String = CStr(CType(v_attrColl.GetNamedItem(gc_AtributeFUNCNAME), Xml.XmlAttribute).Value)
                Select Case Trim(v_strActionFlag)
                    Case gc_ActionInquiry
                        Select Case Trim(v_strFuncName)
                            Case "BranchDeActive"
                                'Cập nhật trạng thái của Branch là DeActive
                                v_lngErrorCode = v_objSystemAdmin.BranchDeActive(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "BranchActive"
                                'Cập nhật trạng thái của Branch là Active
                                v_lngErrorCode = v_objSystemAdmin.BranchActive(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "HostDeActive"
                                'Kiểm tra còn chi nhánh nào đang Active thì thông báo lại để confirm
                                v_lngErrorCode = v_objSystemAdmin.HostDeActive(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "HostActive"
                                'Đặt tham số Host là Active
                                v_lngErrorCode = v_objSystemAdmin.HostActive(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "GetInventory"
                                'Lấy mã khách hàng/số hợp đồng/số tài khoản lưu ký
                                v_lngErrorCode = v_objSystemAdmin.GetInventory(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "CheckOrderStatus"
                                'Kiem tra trang thai lenh doc di
                                'v_lngErrorCode = v_objSystemAdmin.CheckOrderStatus(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "UpdateOrderStatus"
                                'Kiem tra trang thai lenh doc di
                                'v_lngErrorCode = v_objSystemAdmin.UpdateOrderStatus(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "GetSystemTime"
                                'Kiem tra trang thai lenh doc di
                                v_lngErrorCode = v_objSystemAdmin.GetSystemTime(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "BEGINOFDAY"
                                v_lngErrorCode = v_objSystemAdmin.BEGINOFDAY(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "ENDOFDAY"
                                v_lngErrorCode = v_objSystemAdmin.ENDOFDAY(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "UPDATE_ROOM_STATUS"
                                v_lngErrorCode = v_objSystemAdmin.UPDATE_ROOM_STATUS(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "UPDATE_ROOM_STATUS1"
                                v_lngErrorCode = v_objSystemAdmin.UPDATE_ROOM_STATUS1(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "UPDATE_ROOM_STATUS2"
                                v_lngErrorCode = v_objSystemAdmin.UPDATE_ROOM_STATUS2(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "UPDATE_BANK_STATUS"
                                v_lngErrorCode = v_objSystemAdmin.UPDATE_BANK_STATUS(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                                'bangpv
                            Case "GETFTP"
                                v_lngErrorCode = v_objSystemAdmin.GETFTP(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                            Case "SendFTPtoHNX"
                                v_lngErrorCode = v_objSystemAdmin.SendFTPtoHNX(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                                'end bangpv
                                'Case "WriteSessionIn"
                                '    v_lngErrorCode = v_objSystemAdmin.WriteSessionIn(pv_strObjMessage)
                                '    If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                '        ContextUtil.SetAbort()
                                '    Else
                                '        ContextUtil.SetComplete()
                                '    End If
                                '    Return v_lngErrorCode
                                'Case "WriteSessionOut"
                                '    v_lngErrorCode = v_objSystemAdmin.WriteSessionOut(pv_strObjMessage)
                                '    If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                '        ContextUtil.SetAbort()
                                '    Else
                                '        ContextUtil.SetComplete()
                                '    End If
                                '    Return v_lngErrorCode
                                'Case "GetMessages"
                                '    v_lngErrorCode = v_objSystemAdmin.UpdateUserAccess(pv_strObjMessage)
                                '    If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                '        ContextUtil.SetAbort()
                                '    Else
                                '        ContextUtil.SetComplete()
                                '    End If
                                '    Return v_lngErrorCode
                            Case "FAVMENU"
                                v_lngErrorCode = v_objSystemAdmin.FavMenu(pv_strObjMessage)
                                If v_lngErrorCode <> ERR_SYSTEM_OK Then
                                    'ContextUtil.SetAbort()
                                Else
                                    'ContextUtil.SetComplete()
                                End If
                                Return v_lngErrorCode
                        End Select

                    Case gc_ActionAdd

                    Case gc_ActionEdit

                    Case gc_ActionDelete

                End Select
            Else
                Dim v_objMaintain As CoreBusiness.IMaster = Nothing
                If v_strActionFlag <> gc_ActionInquiry Then
                    'Xử lý chung cho các Object kế thừa từ objMaster
                    Select Case v_strObjectName
                        Case OBJNAME_SY_DEFERROR
                            'v_objMaintain = New DEFERROR
                        Case OBJNAME_SA_ALLCODE
                            v_objMaintain = New Sats.SA.AllCode
                        Case OBJNAME_SA_TLGROUPS
                            v_objMaintain = New Sats.SA.TLGROUPS
                        Case OBJNAME_SA_TLPROFILES
                            v_objMaintain = New SA.TLPROFILES
                        Case OBJNAME_SA_TLAUTH
                            v_objMaintain = New SA.TLAUTH
                        Case OBJNAME_SA_CMDAUTH
                            v_objMaintain = New SA.CMDAUTH
                        Case OBJNAME_SA_BRGRP
                            v_objMaintain = New SA.BRGRP
                        Case OBJNAME_RG_RGIS
                            v_objMaintain = New RG.RGIS
                        Case OBJNAME_RG_RGMI
                            v_objMaintain = New RG.RGMI
                        Case OBJNAME_RG_RGSI
                            v_objMaintain = New RG.RGSI
                        Case OBJNAME_RG_RGII
                            v_objMaintain = New RG.RGII
                            'Case OBJNAME_RP_RPLIST
                            'v_objMaintain = New RP.RPLIST
                        Case OBJNAME_MF_ADDMFTOTLTX
                            v_objMaintain = New MF.MF
                        Case OBJNAME_SA_TLTX
                            v_objMaintain = New SA.TLTX
                        Case OBJNAME_RP_RPREPORT
                            v_objMaintain = New RP.Report
                        Case OBJNAME_SA_TLTXUSERAUTH
                            v_objMaintain = New SA.TLTXUSERAUTH
                    End Select
                Else
                    v_objMaintain = New RP.Report
                End If
                'Thực hiện các bước xử lý
                If Not (v_objMaintain Is Nothing) Then
                    Select Case v_strActionFlag
                        Case gc_ActionAdd
                            v_lngErrorCode = v_objMaintain.Add(v_xmlDocument)
                        Case gc_ActionEdit
                            v_lngErrorCode = v_objMaintain.Edit(v_xmlDocument)
                        Case gc_ActionDelete
                            v_lngErrorCode = v_objMaintain.Delete(v_xmlDocument)
                        Case gc_ActionInquiry
                            v_lngErrorCode = v_objMaintain.Inquiry(v_xmlDocument)
                        Case gc_ActionAdhoc
                            v_lngErrorCode = v_objMaintain.Adhoc(v_xmlDocument)
                    End Select
                    pv_strObjMessage = v_xmlDocument.InnerXml
                End If
            End If

            If v_lngErrorCode <> ERR_SYSTEM_OK Then
                'ContextUtil.SetAbort()
            Else
                'ContextUtil.SetComplete()
            End If
            Return v_lngErrorCode
        Catch ex As Exception
            'ContextUtil.SetAbort()
            'LogError.Write("Error source: " & ex.Source & vbNewLine _
            '             & "Error code: System error!" & vbNewLine _
            '             & "Error message: " & ex.Message, EventLogEntryType.Error)
            Throw ex
        End Try
    End Function
    Public Function GetSearch4STP(ByVal message As XmlDocumentEx) As DataSet
        Try
            Dim obj As New CoreBusiness.objMaster
            Return obj.GetSearch4STP(message)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region " IDisposable Support "
    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region
End Class
