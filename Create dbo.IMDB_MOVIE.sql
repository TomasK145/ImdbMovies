USE [ImdbMoviesDb]
GO
select INFO_MESSAGE, count(*) from [dbo].[IMDB_MOVIE] with (nolock) group by INFO_MESSAGE
----> 1538523 --> 1538535 --> 1538456 --> 1540500 --> 1540536 --> 1540555


select max(IMDB_ID_NUM) from [dbo].[IMDB_MOVIE] --4468348


select IMDB_ID_NUM from [dbo].[IMDB_MOVIE] order by IMDB_ID_NUM asc
---------------------------------------------------------------

declare @repeaterCount int, @counter int
set @repeaterCount = 10000
set @counter = 1

IF OBJECT_ID('tempdb..#missingIds') IS NOT NULL DROP TABLE #missingIds 
create table #missingIds(IMDB_ID_NUM int)


while (@counter < @repeaterCount)
begin
	if (select count(*) from [dbo].[IMDB_MOVIE] where IMDB_ID_NUM = @counter) = 0
	begin
		insert into #missingIds(IMDB_ID_NUM) values (@counter)
	end 
	set @counter = @counter + 1
end 

select * from #missingIds
---------------------------------------------------------------
declare @counter int, @maxId int
set @counter = 1
select @maxId = max(IMDB_ID_NUM) from [dbo].[IMDB_MOVIE]  --5468306
--select @counter = max(IMDB_ID_NUM) from [dbo].IMDB_MISSING_MOVIES 

while(@counter < @maxId)
begin 
	insert into dbo.IMDB_MISSING_MOVIES (IMDB_ID_NUM, IS_MISSING) values (@counter, null)
	set @counter = @counter + 1;
end

select max(IMDB_ID_NUM), count(*) from [dbo].IMDB_MISSING_MOVIES 
---------------------------------------------------------------
update mm
set mm.IS_MISSING = 1
from dbo.IMDB_MISSING_MOVIES as mm
left join [dbo].[IMDB_MOVIE] as im on mm.IMDB_ID_NUM = im.IMDB_ID_NUM
where im.IMDB_ID_NUM is null
---------------------------------------------------------------
select top (1000) IMDB_ID_NUM from IMDB_MISSING_MOVIES where IS_MISSING = 1 and IMDB_ID_NUM >= 1