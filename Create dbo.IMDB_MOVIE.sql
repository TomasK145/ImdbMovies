USE [ImdbMoviesDb]
GO
select INFO_MESSAGE, count(*) from [dbo].[IMDB_MOVIE] group by INFO_MESSAGE
