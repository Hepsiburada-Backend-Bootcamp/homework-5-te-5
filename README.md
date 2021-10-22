# Database kurulumu
Gerekli toolların kurulumu:

`dotnet tool install --global dotnet-ef`

Docker ayağa kaldırma (Solution altında çalıştırılmalı):

`docker compose up -d`

Migrationlar ekleme ve database update (API projesinin altında çalıştırılmalı):

`dotnet ef migrations add AddTables -v`

`dotnet ef database update -v`


# Database admin panel erişimi

## Postgres
Docker ayağa kaldırıldıktan sonra `localhost:5050` ile admin paneline erişilir.

```
email: root@hepsinerede.com
password: toor
```
Servers'e sağ tıklayarak Create > Server ile server oluşturma ekranında "General" ve "Connection" sekmelerinden aşağıdaki ayarlamalar yapılır.

```
name: ecommerce

host name: postgresql
port: 5432
username: postgres
password: toor
```
## Mongo
Docker ayağa kaldırıldıktan sonra `localhost:8081` ile admin paneline erişilir.

<br></br>

# 5. Hafta Ödev
Bir önceki hafta geliştirdiğiniz projeye
- Ürünleri ad, fiyat aralığı vb. kriterlerle filtreleme ekleyin
- Ürün listeleme de sayfalama özelliği ekleyin
- Kategori ve ürün için arama özelliği ekleyin
- kullanıcı işlemleri için Asp.NET Core Identity altyapısını kullanın
- api de yetkilendirme işlemleri için JWT kullanın


