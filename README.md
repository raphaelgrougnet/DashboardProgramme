# Garneau Template

## Vues

![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/1.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/2.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/3.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/4.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/5.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/6.png)
![image](https://github.com/raphaelgrougnet/DashboardProgramme/blob/main/images-vues/7.png)

## Stack

- .NET 6
- SQL database

## Dependencies

- FastEndpoint
- Entity Framework Core

## Installation

- Install latest SDK of .NET Core 6 [here] (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Restore nuget package
- Install [Sql Server](https://www.microsoft.com/en-ca/sql-server/sql-server-downloads)
-

Install [SSMS] (https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

- Create dev user with rights to create tables :
    - In `Security` folder, right click on `Logins` and select `New login`
    - Select SQL Server authentication
    - Login name : `dev`H
    - Password : `dev`
    - Click OK
- Install nvm
- Install node 18.16.1
    ```bash
    $ nvm install 18.16.1
    $ nvm use 18.16.1
    ```

### Run front-end Vue app

```bash
# create .env and write this in it
VUE_APP_API_BASE_URL=https://localhost:7101

$ cd .\src\Web\vue-app
# Installs dependencies
$ npm install yarn
$ yarn install

# Compiles and minifies for production
$ yarn build --watch
```

### Run back-end .NET Core 6 app

```bash
$ cd .\src\Web
$ npm install gulp
# This will run both dotnet watch run command and gulp command (for login page css)
$ npm run dev
# You can also run them separately
$ dotnet watch run
$ gulp
```

### Seed

- Default user is `admin@gmail.com` with password `Qwerty123!`

### Migrations

From the Infrastructure assembly:

```bash
$ cd .\src\Persistence

$ dotnet ef migrations add {MigrationName} --startup-project ..\Web\

$ dotnet ef database update --startup-project ../Web/
```

## Instant

An instant represents a moment in time and is always in UTC. Hence, `InstantHelper.GetUtcNow()` will return UTC current
date and time.
However, if you parse a string to an Instant (using `ParseFromString` for example) the Instant will contain the same
date as the string but it will be saved in UTC.
For example,

- If you parse the string `2023-10-12` to an Instant, the Instant object will contain `2023-10-12 00:00:00` and
  not `2023-10-11 20:00:00` (which is UTC equivalent of `2023-10-12 00:00:00`)
- But, `2023-10-11 20:00:00-04` will be saved in database
- But, when getting object from database after saving it, the Instant will be `2023-10-12 00:00:00` and
  not `2023-10-11 20:00:00`
- It is parsed automatically when getting object from database

In conclusion, you don't need to worry about the timezone when parsing a string to an Instant or when getting an Instant
from database. The timezone will be your local timezone.
Also, you don't need (and should not) compare your object's Instant with `InstantHelper.GetUtcNow()` because the Instant
will be in your local timezone and not in UTC.

#### Comparing object's Instant to current date

Although, your object's Instant will be in your local timezone, it will be saved in UTC.

Here's a list of conversion methods offered with Instant and what they do to an Instant that
contains `2023-10-10 00:00:00`

1. `ToDateTimeUtc()` : will return DateTime with `2023-10-10 00:00:00`
2. `ToDateTimeOffset()` : will return DateTime and offset with `2023-10-10 00:00:00 +00:00`

So to compare your object's instant to current date time, I suggest
doing `myObject.ItsInstant.ToDateTimeUtc() < DateTime.Now`
