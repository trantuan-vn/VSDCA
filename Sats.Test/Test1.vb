Imports Sats.ClientCA.ClientBussinessCA
Imports BkavCASign

Module Test1

    Sub Main()
        Dim v_strEncryptXML = "<DataXML>" _
            & "<OrigXML>" & "Hello world" & "</OrigXML>" _
            & "<SignatureXML>" & "Myvq" & "</SignatureXML>" _
            & "</DataXML>"
        Dim v_strOrigXML = ""
        Dim v_strSignNature = ""
        Dim v_intError = ClientCA.ClientBussinessCA.DeCombineData(v_strEncryptXML, _
                                    v_strOrigXML, v_strSignNature)
        If (v_intError <> 0) Then
            Return
        End If
        Console.WriteLine(v_strOrigXML)
        Console.WriteLine(v_strSignNature)
        Dim b = 0


    End Sub

End Module
