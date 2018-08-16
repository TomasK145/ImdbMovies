USE [ImdbMoviesDb]
GO
select INFO_MESSAGE, count(*) from [dbo].[IMDB_MOVIE] group by INFO_MESSAGE
--1286503 --> 1299145

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