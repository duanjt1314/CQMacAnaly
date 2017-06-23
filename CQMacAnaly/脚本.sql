--dataBase 

create table MacTest
(
	terminal_mac	varchar(68),
	detect_time numeric(16)
)

select * from MacTest where detect_time
select * from MacTest where terminal_mac='D8-15-0D-09-25-6C'