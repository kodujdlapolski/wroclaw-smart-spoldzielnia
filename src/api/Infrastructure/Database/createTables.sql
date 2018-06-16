CREATE TABLE Buildings (
    Id SERIAL PRIMARY KEY,
    Name varchar(30) NOT NULL,
    Description varchar(255)
);

CREATE TABLE Services (
    Id SERIAL PRIMARY KEY,
    BuildingId int NOT NULL,
    Name varchar(30) NOT NULL,
    Description varchar(255),
    FOREIGN KEY(BuildingId) REFERENCES Buildings(Id)
);

CREATE TABLE Issues (
    Id SERIAL PRIMARY KEY,
    ServiceId int NOT NULL,
    Description varchar(255),
    Issuer varchar(100) NOT NULL,
    FOREIGN KEY(ServiceId) REFERENCES Services(Id)
);

INSERT INTO Buildings (Name, Description) 
  VALUES ('Building1', 'Test Building 1');
INSERT INTO Buildings (Name, Description) 
  VALUES ('Building2', 'Test Building 2');

INSERT INTO Services (BuildingId, Name, Description) 
  VALUES (1, 'Elevator1', 'Test Elevator 1');
INSERT INTO Services (BuildingId, Name, Description) 
  VALUES (1, 'Internet1', 'Test Internet Cable 1');
INSERT INTO Services (BuildingId, Name, Description) 
  VALUES (2, 'Elevator2', 'Test Elevator 2');

INSERT INTO Issues (ServiceId, Description, Issuer) 
  VALUES(2, 'Urwali od internetu', 'Zofia');