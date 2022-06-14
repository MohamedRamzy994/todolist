create database ToDoList
use[ToDoList]
if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tblToDoListMain')
begin
create table tblToDoList(

Id int identity primary key,
TaskTitle nvarchar(250) not null,
TaskStartDate smalldatetime ,
TaskEndDate smalldatetime,
)
end
if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tblToDoListNotes')
begin
create table tblToDoListNotes(
NoteId int identity primary key,
NoteName nvarchar(max) default 'Task Note Empty',
Id int foreign key references tblToDoList(Id)
)
end

if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tblToDoListState')
begin
create table tblToDoListState(
Id int,
StateId int,
)
end
Alter Table tblToDoListState drop column TaskState
Alter Table tblToDoListState add StateId int

if not exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tblToDoListStateValues')
begin
create table tblToDoListStateValues(
StateId int primary key,
StateValue nvarchar(100),
)
end

insert into tblToDoListStateValues values (1,'completed')
insert into tblToDoListStateValues values (2,'pending')
insert into tblToDoListStateValues values (3,'canceled')


if not exists (select * from sys.objects where type='P' and NAME='stAddTask')
begin
Alter proc stAddTask
@TaskTitle nvarchar(250),
@TaskStartDate smalldatetime,
@TaskEndDate smalldatetime,
@TaskNotes nvarchar(max)

as
begin
declare @insertedId int

   insert into tblToDoList values(@TaskTitle,@TaskStartDate,@TaskEndDate)
   select @insertedId= SCOPE_IDENTITY()



 insert into tblToDoListNotes values(@TaskNotes,@insertedId)
 insert into tblToDoListState(Id,StateId) values(@insertedId,2)


end

end

if not exists (select * from sys.objects where type='P' and NAME='stDeleteTask')
begin

Create proc stDeleteTask
@Id int
as
begin

delete from tblToDoList where Id = @Id
end

end


if not exists (select * from sys.objects where type='P' and NAME='stSelectAllTasks')
begin

Alter proc stSelectAllTasks
as
BEGIN

select main.* , states.StateValue from tblToDoList as main 
join tblToDoListState
on tblToDoListState.Id = main.Id
join tblToDoListStateValues as states
on tblToDoListState.StateId = states.StateId
END
END

if not exists (select * from sys.objects where type='P' and NAME='stSelectTaskById')
begin

Alter proc stSelectTaskById
@Id int
as
BEGIN

select * from tblToDoList
inner join tblToDoListNotes
on tblToDoListNotes.Id =tblToDoList.Id and tblToDoList.Id = @Id
END
END

if not exists (select * from sys.objects where type='P' and NAME='stEditTaskById')
begin

Create proc stEditTaskById
@Id int,
@TaskTitle nvarchar(250),
@TaskStartDate smalldatetime ,
@TaskEndDate smalldatetime,
@TaskNotes nvarchar(max)
as
BEGIN

update tblToDoList 
set TaskTitle = @TaskTitle,
TaskStartDate=@TaskStartDate,
TaskEndDate=@TaskEndDate
where tblToDoList.Id = @Id;

update tblToDoListNotes
set  NoteName=@TaskNotes
where Id=@Id

END
END


DBCC CHECKIDENT ('tblToDoList', RESEED, 0);
DBCC CHECKIDENT ('tblToDoListNotes', RESEED, 0);


exec stAddTask 'Task one','2020-08-30 12:22:00','2020-08-30 2:22:00','hello this is my first task note'
exec stEditTaskById 1,'Task one modification','2020-08-30 12:22:00','2020-08-30 2:22:00','hello this is my first task note modification'
exec stDeleteTask 13
exec stSelectAllTasks 
exec stSelectTaskById 1



select * from dbo.tblToDoList
select * from dbo.tblToDoListNotes
select * from dbo.tblToDoListStateValues
select * from dbo.tblToDoListState

select name, type_desc from sys.objects 
WHERE type in ( 'C', 'D', 'F', 'L', 'P', 'PK', 'RF', 'TR', 'UQ', 'V', 'X' ) 
union
select name, type_desc from sys.indexes
order by name