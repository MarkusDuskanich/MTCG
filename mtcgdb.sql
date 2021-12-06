--create database mtcgdb;

--\c mtcgdb

drop table if exists packages cascade;
drop table if exists tradeoffers cascade;
drop table if exists cards cascade;
drop table if exists users cascade;

create table
if not exists
users(
id uuid primary key,
username text unique,
name text null,
password text not null,
bio text default null,
image text default null,
coins integer default 20,
wins integer default 0,
losses integer default 0,
gamesplayed integer default 0,
token text default null,
tokenexpiration text default null,
lastlogin text default null,
loginstreak integer default 0,
version integer not null default 1);

create table
if not exists
cards(
id uuid primary key,
userid uuid not null,
name text not null,
damage integer default 0,
indeck boolean default false,
istradeoffer boolean default false,
version integer not null default 1,
constraint fk_cards_users foreign key(userid) references users(id) on delete cascade);

create table
if not exists
tradeoffers(
id uuid primary key,
cardid uuid not null,
userid uuid not null,
mustbespell boolean default false,
mindamage integer default 0,
element text default null,
version integer not null default 1,
constraint fk_tradeoffers_cards foreign key(cardid) references cards(id) on delete cascade,
constraint fk_tradeoffers_users foreign key(userid) references users(id) on delete cascade);

create table
if not exists
packages(
id uuid not null,
packagenum integer not null,
name text not null,
damage integer not null,
version integer not null default 1);




