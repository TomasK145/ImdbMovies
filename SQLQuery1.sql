﻿select top 100 *
from dbo.IMDB_MOVIE
where IMDB_ID_NUM in (88245,88246,88247,88248,88249,88250)

select top 100 *
from dbo.IMDB_MISSING_MOVIES
where IMDB_ID_NUM in (88245,88246,88247,88248,88249,88250)