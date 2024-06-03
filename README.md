# Microblog

## Instalācijas instrukcija

Lai uzstādītu un palaistu Microblog sistēmu, ir jāveic sekojošas darbības:

### Prasības

- **Visual Studio**: Jābūt instalētai Visual Studio attīstības videi.
- **.NET Core SDK 8.0 versija**: Pārliecinieties, ka ir instalēta .NET Core SDK 8.0 versija.
- **SQL Server**: Nepieciešams SQL Server darbam ar datu bāzēm.

### Instalācijas soļi

1. **Projekta klonēšana**:
    ```sh
    git clone https://github.com/truearogog/MicroBlog.git
    ```

2. **Datu bāzu atjaunošana no dublējumiem**:
    - Atjaunojiet datu bāzes no pilnām dublējumkopijām, izmantojot SQL Management Studio vai citu piemērotu rīku.
    - Pilnās dublējumkopijas atrodas mapē `...\MicroBlog\demo`.
    - Izmantojiet šādas komandas, lai atjaunotu datu bāzes. ... vieta vajag iekopēt ceļu līdz projekta mapei:
        ```sql
        RESTORE DATABASE [microblog.dev] FROM DISK = '...\MicroBlog\demo\microblog_dev_full.bak' WITH REPLACE;
        RESTORE DATABASE [microblog.identity.dev] FROM DISK = '...\MicroBlog\demo\microblog_identity_dev_full.bak' WITH REPLACE;
        ```

3. **Attēlu pārvietošana**:
    - ... Vietā vajag iekopēt ceļu līdz projekta mapei:
    - Pārvietojiet attēlus no mapes `...\MicroBlog\demo\demouploads` uz mapi `...\MicroBlog\src\MicroBlog.Web\wwwroot\uploads`.

4. **Projekta palaišana**:
    - Lai palaistu tīmekļa vietni, atveriet termināli projekta `...\MicroBlog.Web` mapē un izpildiet komandu:
        ```sh
        dotnet run
        ```

## Dokumentācija

Papildu informāciju par projekta uzbūvi un izmantošanu var atrast projekta dokumentācijas mapē `docs`.

## Lietotāja pieeja

Galvenais lietotājs ar kuru var apskatīt visu sistēmu un kā tas darbojas:
login: admin@test.com
pass: Vgt!7$yGMJBTBtR
