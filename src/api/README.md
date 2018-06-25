# Api

Api mające służyć do dostarczania danych niezbędnych do funkcjonowania bota

### Prerequisites

* Baza danych PostgreSQL 
* .NET Core 2.0
* Windows :) - z uwagi na słabe wsparcie System.Data.Sql w aktualnej wersji .Net Core, kompilacja wymaga Windowsa, na runtime może być linux
* Opcjonalnie do dev i koniecznie do deploy: docker

### Installing

1. Postaw bazę PostgreSQL. Ze względu na użycie Type Providera baza developerska jest konieczna do zbudowania kodu. Do zarządzania bazą na Heroku także potrzebna jest lokalna instalacja postgresa. Kod zakłada że connection string znajduje się w zmiennej środowiskowej DATABASE_URL, przykładowy format :
  
  ```postgres://Dev:dev@localhost:5432/spoldzielnia```

  Gdzie:

  * **spoldzielnia = nazwa bazy** lub wartość zmiennej środowiskowej "dbName"
  * **Dev = username** lub wartość zmiennej środowiskowej "dbUserName"
  * **dev = password** lub wartość zmiennej środowiskowej "dbPassword"
  * struktura tabel odpowiada tej z **Infrastructure/Database/createTables.sql**
  * username powinien mieć granty do tych tabel

2. Wykonaj w folderze /Api 

```dotnet run```

3. Opcjonalnie: folderze /Api wykonaj

```dotnet publish -c Release```

Następnie w głównym folderze (tym, który zawiera Dockerfile)

```docker build -t api .```

Container nie będzie widział zmiennych środowiskowych i trzeba mu je przekazać w poleceniu ```run```. Dodatkowo jeśli chcemy się połączyć z bazą zainstalowaną poza kontenerem to zmienia się adres (localhost != container localhost).

```docker run -p 8181:80 -e DATABASE_URL=postgres://Dev:dev@host.docker.internal:5432/spoldzielnia api```

Api będzie nasłuchiwać na porcie 8181 (w konsoli się wyświetli że 80, jednak to port lokalny dla kontenera).

4. Listę uruchomionych containerów można dostać komendą

```docker container ls```

container można zatrzymać poleceniem

```docker stop <ID>```

a usunąć 

```docker rm <ID>```

4. Deploy na Heroku. W katalogu z dockerfile wykonujemy (gdzie <app> to nazwa naszej aplikacji w heroku):

```heroku container:login```

```heroku container:push web -a <app>```

```heroku container:release web -a <app>```

## Testy

Testy są uruchamiane poleceniem ```dotnet run``` na projekcie testowym