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
bio text default null,
image text default null,
coins integer default 20,
wins integer default 0,
losses integer default 0,
token text default null,
tokenexpiration bigint default 0,
lastlogin timestamp default null,
loginstreak integer default 0);

create table
if not exists
cards(
id uuid primary key,
userid uuid not null,
name text not null,
damage integer default 0,
indeck boolean default false,
istradeoffer boolean default false,
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
constraint fk_tradeoffers_cards foreign key(cardid) references cards(id) on delete cascade,
constraint fk_tradeoffers_users foreign key(userid) references users(id) on delete cascade);

create table
if not exists
packages(
cardnumber serial,
id uuid not null,
name text not null,
damage integer not null);




