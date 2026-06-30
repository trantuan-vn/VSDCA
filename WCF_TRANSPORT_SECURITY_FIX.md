# Huong dan bat ma hoa transport WCF

## 1. Muc tieu

Tai lieu nay huong dan sua cau hinh WCF de khong con truyen request qua `net.tcp` voi `security mode="None"`.

Sau khi sua, cac `netTcpBinding` chinh se dung:

```xml
<security mode="Transport">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
</security>
```

Y nghia:

- `mode="Transport"`: bat bao mat o tang transport cho `netTcpBinding`.
- `clientCredentialType="Windows"`: dung Windows authentication giua client va service.
- `protectionLevel="EncryptAndSign"`: ma hoa va ky goi tin tren duong truyen.

Luu y: Cach cau hinh nay phu hop khi cac may chay trong domain Windows hoac co trust phu hop. Neu moi truong khong dung Windows authentication (client khong join domain), xem phuong an rieng tai `WCF_TRANSPORT_SECURITY_FIX_NO_DOMAIN.md`.

---

## 2. Danh sach file can sua

| Vai tro | File |
| --- | --- |
| HOST service | `Sats.HOSTWindowsService\app.config` |
| HOST report service | `Sats.HOSTReportService\app.config` |
| BDS service | `Sats.BDSWindowsService\app.config` |
| Client app | `Sats.App\app.config` |
| BDS channel proxy | `Sats.BDSChannel\app.config` |
| HOST channel proxy | `Sats.HOSTChannel\app.config` |
| HOST report channel proxy | `Sats.HOSTReportChannel\app.config` |
| Deploy config BDS | `Sats.BDSWindowsService.exe.config` |
| Deploy config HOST | `Sats.HOSTWindowsService.exe.config` |

Khong nen sua cac file trong `bin`, `bin1`, `Debug`, backup copy cu neu do chi la artifact build. Khi build/deploy lai, cac file runtime `.exe.config` phai duoc sinh hoac copy tu config da sua.

---

## 3. Cach sua tung nhom file

### 3.1. Service binding: HOST va HOST Report

Ap dung cho:

- `Sats.HOSTWindowsService\app.config`
- `Sats.HOSTReportService\app.config`
- `Sats.HOSTWindowsService.exe.config`

Tim binding dang co dang:

```xml
<binding name="tcpBinding" ...>
  <security mode="None">
  </security>
  <readerQuotas ... />
  <reliableSession enabled="true" inactivityTimeout="00:30:00" />
</binding>
```

Sua thanh:

```xml
<binding name="tcpBinding" ...>
  <security mode="Transport">
    <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  </security>
  <readerQuotas ... />
  <reliableSession enabled="true" inactivityTimeout="00:30:00" />
</binding>
```

Ghi chu:

- Giu nguyen cac thuoc tinh tren `<binding>`.
- Giu nguyen `readerQuotas`, `reliableSession`, timeout, max size.
- Chi doi `security mode` va them `<transport ... />`.

---

### 3.2. BDS service binding va client binding

Ap dung cho:

- `Sats.BDSWindowsService\app.config`
- `Sats.BDSWindowsService.exe.config`

Trong file nay co nhieu binding:

- `tcpBinding`: service binding cua BDS.
- `NetTcpBinding_IWCF`: BDS goi sang HOST.
- `NetTcpBinding_IReportWCF`: BDS goi sang HOST Report.

Voi binding dang rong:

```xml
<security mode="None">
</security>
```

Sua thanh:

```xml
<security mode="Transport">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
</security>
```

Voi binding da co san `<transport>` nhung dang bi vo hieu:

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Sua thanh:

```xml
<security mode="Transport">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Ghi chu:

- `<message clientCredentialType="Windows" />` co the giu lai de giam thay doi, du `Transport` chu yeu dung `<transport>`.
- Cac endpoint phai dung bindingConfiguration da sua, vi server va client phai khop security mode.

---

### 3.3. Client app goi BDS

Ap dung cho:

- `Sats.App\app.config`
- `Sats.BDSChannel\app.config`

Tim:

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Sua thanh:

```xml
<security mode="Transport">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Ghi chu:

- `Sats.App\app.config` la config cua ung dung client chinh.
- `Sats.BDSChannel\app.config` la config cua project channel/proxy. Neu build output lay config tu project nao, can dam bao output `.exe.config` cuoi cung cung co `Transport`.

---

### 3.4. HOST channel va HOST Report channel

Ap dung cho:

- `Sats.HOSTChannel\app.config`
- `Sats.HOSTReportChannel\app.config`

Tim cac binding:

- `NetTcpBinding_IWCF`
- `NetTcpBinding_IWCF1`
- `NetTcpBinding_IReportWCF`

Sua:

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Thanh:

```xml
<security mode="Transport">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

Ghi chu:

- Cac channel nay la proxy client goi sang HOST/HOST Report, nen phai khop voi service binding o `Sats.HOSTWindowsService` va `Sats.HOSTReportService`.

---

## 4. Kiem tra sau khi sua

Chay tim kiem trong source config:

```powershell
rg '<security mode="None"' -g app.config
```

Ket qua mong doi:

```text
No matches found
```

Chay tim kiem toan bo config:

```powershell
rg '<security mode="None"' -g '*.config'
```

Neu con match trong `bin`, `bin1`, `Debug`, backup copy cu:

- Khong can sua neu do la artifact cu.
- Can build/deploy lai de file runtime `.exe.config` duoc cap nhat.
- Neu moi truong deploy dang copy truc tiep cac file nay, phai cap nhat ban deploy tu config da sua.

Kiem tra cac binding da bat `Transport`:

```powershell
rg '<security mode="Transport"' -g '*.config'
```

---

## 5. Thu tu deploy de tranh mismatch binding

Khuyen nghi deploy theo thu tu:

1. HOST service:
   - `Sats.HOSTWindowsService.exe.config`
   - restart HOST Windows Service.
2. HOST Report service:
   - `Sats.HOSTReportService.exe.config`
   - restart HOST Report Windows Service.
3. BDS service:
   - `Sats.BDSWindowsService.exe.config`
   - restart BDS Windows Service.
4. Client app:
   - cap nhat `Sats.exe.config` hoac config runtime tu `Sats.App\app.config`.

Quan trong: Server va client cua cung mot endpoint phai cung dung `Transport`. Neu mot ben van `None`, ket noi WCF se fail do binding security khong khop.

---

## 6. Checklist test

Sau khi deploy, test toi thieu cac luong sau:

- Dang nhap thuong `Login()`.
- Dang nhap CA `LoginCA()`.
- Giao dich thuong qua `Send()`.
- Giao dich CA qua `SendCA()`.
- Bao cao qua HOST Report.
- Save file/report neu co dung `SaveFileRptCA`.

Neu gap loi ket noi WCF, kiem tra:

- Service va client co cung `security mode="Transport"` khong.
- May client/service co cung domain hoac trust Windows khong.
- Tai khoan chay Windows Service co quyen authenticate qua Windows security khong.
- Endpoint address/port co dung khong.
- Firewall co cho phep port `net.tcp` khong.

---

## 7. Cach xac minh da fix viec bat plain text tren duong truyen

Dung Wireshark hoac cong cu sniff tren duong Client -> BDS -> HOST:

- Truoc khi sua: co the thay du lieu WCF khong duoc transport encrypt; voi luong `Send()`, payload chi nen nen nen co the khoi phuc XML.
- Sau khi sua: traffic `net.tcp` phai duoc bao ve boi transport security, khong doc duoc XML/password nghiep vu o dang plain text tren wire.

Luu y: Server van phai giai nen/giai ma thanh XML trong memory de xu ly nghiep vu. Transport security chi bao ve du lieu tren duong truyen, khong tu dong mask log.

---

## 8. Viec nen lam tiep theo

Bat transport security khong thay the cac fix con lai. Nen tiep tuc:

1. Giam log payload:
   - Thay `AppLogger.Info(pv_strMessage)` bang log metadata nhu `TLTXCD`, `TXNUM`, `TLID`, status.
2. Bat application encryption cho `Send()` neu can defense-in-depth:
   - Client: `Sats.BDSChannel\WCF\BDSWCFChannel.vb`.
   - Server: `Sats.HOSTWindowsService\WCF\HOSTWCFService.vb`.
3. Audit cac file config deploy:
   - Dam bao moi `.exe.config` tren server production da dung `Transport`.
4. Neu khong dung Windows domain:
   - Chuyen sang phuong an `clientCredentialType="None"` tai `WCF_TRANSPORT_SECURITY_FIX_NO_DOMAIN.md`.
   - Hoac chuyen sang certificate transport neu can xac thuc may client:

```xml
<security mode="Transport">
  <transport clientCredentialType="Certificate" protectionLevel="EncryptAndSign" />
</security>
```

Khi dung certificate transport, can bo sung cau hinh service certificate/client certificate va trust certificate tren cac may lien quan.
