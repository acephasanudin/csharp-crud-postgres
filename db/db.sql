/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : PostgreSQL
 Source Server Version : 160001 (160001)
 Source Host           : localhost:5432
 Source Catalog        : Demo
 Source Schema         : public

 Target Server Type    : PostgreSQL
 Target Server Version : 160001 (160001)
 File Encoding         : 65001

 Date: 09/03/2024 13:46:22
*/


-- ----------------------------
-- Sequence structure for students_id_seq
-- ----------------------------
DROP SEQUENCE IF EXISTS "public"."students_id_seq";
CREATE SEQUENCE "public"."students_id_seq" 
INCREMENT 1
MINVALUE  1
MAXVALUE 2147483647
START 1
CACHE 1;

-- ----------------------------
-- Table structure for students
-- ----------------------------
DROP TABLE IF EXISTS "public"."students";
CREATE TABLE "public"."students" (
  "st_id" int4 NOT NULL DEFAULT nextval('students_id_seq'::regclass),
  "st_firstName" varchar(255) COLLATE "pg_catalog"."default",
  "st_midName" varchar(255) COLLATE "pg_catalog"."default",
  "st_lastName" varchar(255) COLLATE "pg_catalog"."default"
)
;

-- ----------------------------
-- Function structure for st_delete
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."st_delete"("_id" int4);
CREATE OR REPLACE FUNCTION "public"."st_delete"("_id" int4)
  RETURNS "pg_catalog"."int4" AS $BODY$

begin
 delete from students
 where "st_id" = _id;
 if found then --deleted successfully
  return 1;
 else
  return 0;
 end if;
end

$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- ----------------------------
-- Function structure for st_insert
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."st_insert"("_firstname" varchar, "_midname" varchar, "_lastname" varchar);
CREATE OR REPLACE FUNCTION "public"."st_insert"("_firstname" varchar, "_midname" varchar, "_lastname" varchar)
  RETURNS "pg_catalog"."int4" AS $BODY$

begin
 insert into students("st_firstName", "st_midName", "st_lastName")
 values(_firstname, _midName, _lastName);
 if found then --inserted successfully
  return 1;
 else return 0; -- inserted fail
 end if;
end

$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- ----------------------------
-- Function structure for st_select
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."st_select"();
CREATE OR REPLACE FUNCTION "public"."st_select"()
  RETURNS TABLE("id" int4, "_firstname" varchar, "_midname" varchar, "_lastname" varchar) AS $BODY$

 begin
  return query
 select "st_id", "st_firstName", "st_midName", "st_lastName" from students order by "st_id";
 end
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100
  ROWS 1000;

-- ----------------------------
-- Function structure for st_update
-- ----------------------------
DROP FUNCTION IF EXISTS "public"."st_update"("_id" int4, "_firstname" varchar, "_midname" varchar, "_lastname" varchar);
CREATE OR REPLACE FUNCTION "public"."st_update"("_id" int4, "_firstname" varchar, "_midname" varchar, "_lastname" varchar)
  RETURNS "pg_catalog"."int4" AS $BODY$

 begin
  update students
 set 
  "st_firstName" = _firstname,
  "st_midName" = _midname,
  "st_lastName" = _lastname
 where "st_id" = _id;
 if found then --updated successfully
  return 1;
 else --updated fail
  return 0;
 end if;
 end
 
$BODY$
  LANGUAGE plpgsql VOLATILE
  COST 100;

-- ----------------------------
-- Alter sequences owned by
-- ----------------------------
ALTER SEQUENCE "public"."students_id_seq"
OWNED BY "public"."students"."st_id";
SELECT setval('"public"."students_id_seq"', 5, true);

-- ----------------------------
-- Primary Key structure for table students
-- ----------------------------
ALTER TABLE "public"."students" ADD CONSTRAINT "students_pkey" PRIMARY KEY ("st_id");
