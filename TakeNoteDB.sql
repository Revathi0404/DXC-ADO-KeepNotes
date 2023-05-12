Create Database TakeNoteDB
create table Note
(
Id int identity(1,1) Primary key,
Title varchar(60),
Description varchar(50),
Date datetime
) 
Drop table Note
select * from Note