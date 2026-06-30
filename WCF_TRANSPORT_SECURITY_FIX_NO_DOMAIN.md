# Huong dan bat ma hoa transport WCF khi client khong join domain

Phuong an nay danh cho moi truong **client khong join domain Windows**. Khong dung `clientCredentialType="Windows"`.

Neu moi truong co domain/trust Windows, xem phuong an tai `WCF_TRANSPORT_SECURITY_FIX.md`.

---

## 1. Muc tieu

Sua cau hinh WCF de khong con truyen request qua `net.tcp` voi `security mode="None"`, dong thoi **khong yeu cau client authenticate bang Windows/domain**.

Cau hinh chuan:

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

Y nghia:

| Thanh phan | Mo ta |
| --- | --- |
| `mode="Transport"` | Bat bao mat o tang transport cho `netTcpBinding` |
| `clientCredentialType="None"` | Khong xac thuc client bang Windows/domain tai tang WCF |
| `protectionLevel="EncryptAndSign"` | Ma hoa va ky goi tin tren duong truyen |

Luu y quan trong:

- Phuong an nay **ma hoa duong truyen**, khong thay the xac thuc nguoi dung.
- Xac thuc nguoi dung van do cac luong san co: `Login()`, `LoginCA()`, token, CA application-level.
- Khong can cau hinh certificate thumbprint hay trust store cho tung client.
- Neu can xac thuc may client bang certificate, xem muc 9 (phuong an nang cao).

---

## 2. So sanh voi phuong an Windows domain

| Tieu chi | Windows (`WCF_TRANSPORT_SECURITY_FIX.md`) | Khong domain (file nay) |
| --- | --- | --- |
| Client join domain | Bat buoc / khuyen nghi | Khong can |
| `clientCredentialType` | `Windows` | `None` |
| Xac thuc may client WCF | Co (Windows) | Khong |
| Ma hoa transport | Co | Co |
| Do phuc tap trien khai | Trung binh | Thap hon |
| Phu hop VSDCA hien tai | Domain noi bo | Client phan tan / workgroup |

---

## 3. Danh sach file can sua

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

Khong sua cac file trong `bin`, `bin1`, `Debug`, backup copy cu neu do chi la artifact build. Khi build/deploy lai, file runtime `.exe.config` phai duoc sinh hoac copy tu config da sua.

---

## 4. Cach sua tung nhom file

### 4.1. Service binding: HOST va HOST Report

Ap dung cho:

- `Sats.HOSTWindowsService\app.config`
- `Sats.HOSTReportService\app.config`
- `Sats.HOSTWindowsService.exe.config`

**Truoc:**

```xml
<binding name="tcpBinding" ...>
  <security mode="None">
  </security>
  <readerQuotas ... />
  <reliableSession enabled="true" inactivityTimeout="00:30:00" />
</binding>
```

**Sau:**

```xml
<binding name="tcpBinding" ...>
  <security mode="Transport">
    <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
  </security>
  <readerQuotas ... />
  <reliableSession enabled="true" inactivityTimeout="00:30:00" />
</binding>
```

Ghi chu: Giu nguyen timeout, `readerQuotas`, `reliableSession`, max size. Chi doi khoi `<security>`.

---

### 4.2. BDS service binding va client binding

Ap dung cho:

- `Sats.BDSWindowsService\app.config`
- `Sats.BDSWindowsService.exe.config`

Trong file nay co 3 binding:

- `tcpBinding` — service binding cua BDS
- `NetTcpBinding_IWCF` — BDS goi sang HOST
- `NetTcpBinding_IReportWCF` — BDS goi sang HOST Report

**Truong hop A — binding rong:**

```xml
<security mode="None">
</security>
```

**Sau:**

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

**Truong hop B — binding co Windows credential cu (dang bi vo hieu):**

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

**Sau:**

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

Ghi chu:

- **Bo** `<message clientCredentialType="Windows" />` de tranh phu thuoc domain.
- Server va client cung endpoint phai cung `Transport` + `clientCredentialType="None"`.

---

### 4.3. Client app goi BDS

Ap dung cho:

- `Sats.App\app.config`
- `Sats.BDSChannel\app.config`

**Truoc:**

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

**Sau:**

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

Ghi chu:

- `Sats.App\app.config` la config ung dung client chinh.
- `Sats.BDSChannel\app.config` la config project channel/proxy.
- Dam bao `Sats.exe.config` runtime sau build cung co cau hinh nay.

---

### 4.4. HOST channel va HOST Report channel

Ap dung cho:

- `Sats.HOSTChannel\app.config`
- `Sats.HOSTReportChannel\app.config`

Cac binding can sua:

- `NetTcpBinding_IWCF`
- `NetTcpBinding_IWCF1`
- `NetTcpBinding_IReportWCF`

**Truoc:**

```xml
<security mode="None">
  <transport clientCredentialType="Windows" protectionLevel="EncryptAndSign" />
  <message clientCredentialType="Windows" />
</security>
```

**Sau:**

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

Ghi chu: Cac channel nay la proxy client goi sang HOST/HOST Report, phai khop voi service binding o `Sats.HOSTWindowsService` va `Sats.HOSTReportService`.

---

## 5. Mau cau hinh hoan chinh (tham khao)

### HOST service (`tcpBinding`)

```xml
<netTcpBinding>
  <binding name="tcpBinding" portSharingEnabled="True"
           maxBufferSize="2147483647" maxReceivedMessageSize="2147483647"
           maxBufferPoolSize="2147483647" transferMode="Buffered"
           closeTimeout="00:00:10" openTimeout="00:00:10"
           receiveTimeout="00:30:00" sendTimeout="00:30:00" maxConnections="300">
    <security mode="Transport">
      <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
    </security>
    <readerQuotas maxArrayLength="2147483647" maxBytesPerRead="2147483647"
                  maxStringContentLength="2147483647" />
    <reliableSession enabled="true" inactivityTimeout="00:30:00" />
  </binding>
</netTcpBinding>
```

### Client app goi BDS (`NetTcpBinding_IWCF`)

```xml
<netTcpBinding>
  <binding name="NetTcpBinding_IWCF" ...>
    <readerQuotas ... />
    <reliableSession ordered="true" inactivityTimeout="00:30:00" enabled="true" />
    <security mode="Transport">
      <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
    </security>
  </binding>
</netTcpBinding>
<client>
  <endpoint address="net.tcp://localhost:8100/host"
            binding="netTcpBinding"
            bindingConfiguration="NetTcpBinding_IWCF"
            contract="BDSService.IWCF"
            name="NetTcpBinding_IWCF" />
</client>
```

---

## 6. Kiem tra sau khi sua

Tim `security mode="None"` trong source config:

```powershell
rg '<security mode="None"' -g app.config
```

Ket qua mong doi: `No matches found`

Tim `clientCredentialType="Windows"` trong source config (phuong an nay khong dung Windows):

```powershell
rg 'clientCredentialType="Windows"' -g app.config
```

Ket qua mong doi: `No matches found`

Xac nhan da bat Transport + None:

```powershell
rg 'clientCredentialType="None"' -g app.config
```

Neu con match trong `bin`, `bin1`, `Debug`, backup copy cu:

- Khong can sua neu do la artifact cu.
- Build/deploy lai de cap nhat `.exe.config` runtime.

---

## 7. Thu tu deploy

1. **HOST service** — `Sats.HOSTWindowsService.exe.config` → restart service
2. **HOST Report** — `Sats.HOSTReportService.exe.config` → restart service
3. **BDS service** — `Sats.BDSWindowsService.exe.config` → restart service
4. **Client app** — cap nhat `Sats.exe.config` tu `Sats.App\app.config`

Quan trong: Ca hai dau cung endpoint phai cung:

```xml
<security mode="Transport">
  <transport clientCredentialType="None" protectionLevel="EncryptAndSign" />
</security>
```

Neu mot ben van `mode="None"` hoac dung `Windows`, ket noi WCF se fail do binding security khong khop.

---

## 8. Checklist test

Sau khi deploy, test toi thieu:

- [ ] Dang nhap thuong `Login()`
- [ ] Dang nhap CA `LoginCA()`
- [ ] Giao dich thuong `Send()`
- [ ] Giao dich CA `SendCA()`
- [ ] Bao cao HOST Report
- [ ] `SaveFileRptCA` (neu co dung)

Neu loi ket noi WCF, kiem tra:

- [ ] Hai dau cung `security mode="Transport"`
- [ ] Hai dau cung `clientCredentialType="None"`
- [ ] Endpoint address/port dung
- [ ] Firewall mo port `net.tcp` (8100, 8200, 8000...)
- [ ] File `.exe.config` runtime da duoc cap nhat (khong dung ban cu trong `bin`)

---

## 9. Xac minh da fix plain text tren wire

Dung Wireshark hoac cong cu sniff tren duong Client → BDS → HOST:

| Trang thai | Ket qua mong doi |
| --- | --- |
| Truoc khi sua | Co the doc payload WCF; luong `Send()` chi nen, de khoi phuc XML |
| Sau khi sua | Traffic `net.tcp` duoc transport encrypt, khong doc duoc XML/password nghiep vu plain text |

Luu y: Server van giai nen/giai ma thanh XML trong memory de xu ly. Transport security chi bao ve tren duong truyen, khong tu dong mask log.

---

## 10. Viec nen lam tiep theo

Transport security khong thay the cac fix con lai:

1. **Giam log payload** — `Sats.HOSTWindowsService\WCF\HOSTWCFService.vb`: thay `AppLogger.Info(pv_strMessage)` bang metadata (`TLTXCD`, `TXNUM`, `TLID`).
2. **Application encryption cho `Send()`** — neu can defense-in-depth:
   - Client: `Sats.BDSChannel\WCF\BDSWCFChannel.vb`
   - Server: `Sats.HOSTWindowsService\WCF\HOSTWCFService.vb`
3. **Audit deploy** — dam bao moi `.exe.config` production dung `Transport` + `None`.
4. **Phuong an nang cao: mutual certificate** — neu can xac thuc may client:

```xml
<security mode="Transport">
  <transport clientCredentialType="Certificate" protectionLevel="EncryptAndSign" />
</security>
```

Can bo sung service certificate, client certificate, trust store va endpoint behavior. Khong chuyen sang `Certificate` khi chua co ke hoach cap phat/quan ly certificate.

---

## 11. Mapping file da ap dung phuong an nay

Cac file sau trong repo da duoc cau hinh theo phuong an `clientCredentialType="None"`:

- `Sats.HOSTWindowsService\app.config`
- `Sats.HOSTReportService\app.config`
- `Sats.BDSWindowsService\app.config`
- `Sats.App\app.config`
- `Sats.BDSChannel\app.config`
- `Sats.HOSTChannel\app.config`
- `Sats.HOSTReportChannel\app.config`
- `Sats.BDSWindowsService.exe.config`
- `Sats.HOSTWindowsService.exe.config`

Neu can quay lai phuong an Windows domain, doi `clientCredentialType="None"` thanh `Windows` va them lai `<message clientCredentialType="Windows" />` o cac client binding, theo `WCF_TRANSPORT_SECURITY_FIX.md`.
