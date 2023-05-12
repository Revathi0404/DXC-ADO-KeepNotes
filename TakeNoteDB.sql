Create Database TakeNoteDB
create table notes
(
id int primary key identity,
title varchar(50) not null,
description varchar(500) not null,
date datetime not null
) 
select * from notes