--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.3

-- Started on 2021-08-02 21:36:23

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 203 (class 1259 OID 113522)
-- Name: AcademicPerformance; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."AcademicPerformance" (
    id integer NOT NULL,
    name character varying,
    description character varying,
    code character varying
);


ALTER TABLE public."AcademicPerformance" OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 113520)
-- Name: AcademicPerformance_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."AcademicPerformance_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."AcademicPerformance_id_seq" OWNER TO postgres;

--
-- TOC entry 3017 (class 0 OID 0)
-- Dependencies: 202
-- Name: AcademicPerformance_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."AcademicPerformance_id_seq" OWNED BY public."AcademicPerformance".id;


--
-- TOC entry 201 (class 1259 OID 113511)
-- Name: Sex; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Sex" (
    id integer NOT NULL,
    name character varying,
    description character varying,
    code character varying
);


ALTER TABLE public."Sex" OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 113509)
-- Name: Sex_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Sex_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Sex_id_seq" OWNER TO postgres;

--
-- TOC entry 3018 (class 0 OID 0)
-- Dependencies: 200
-- Name: Sex_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Sex_id_seq" OWNED BY public."Sex".id;


--
-- TOC entry 205 (class 1259 OID 113533)
-- Name: Student; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Student" (
    id integer NOT NULL,
    "surName" character varying,
    "firstName" character varying,
    "secondName" character varying,
    dob timestamp without time zone,
    "idSex" integer NOT NULL,
    "idAcademicPerformance" integer
);


ALTER TABLE public."Student" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 113531)
-- Name: Student_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public."Student_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Student_id_seq" OWNER TO postgres;

--
-- TOC entry 3019 (class 0 OID 0)
-- Dependencies: 204
-- Name: Student_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public."Student_id_seq" OWNED BY public."Student".id;


--
-- TOC entry 2866 (class 2604 OID 113525)
-- Name: AcademicPerformance id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AcademicPerformance" ALTER COLUMN id SET DEFAULT nextval('public."AcademicPerformance_id_seq"'::regclass);


--
-- TOC entry 2865 (class 2604 OID 113514)
-- Name: Sex id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Sex" ALTER COLUMN id SET DEFAULT nextval('public."Sex_id_seq"'::regclass);


--
-- TOC entry 2867 (class 2604 OID 113536)
-- Name: Student id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student" ALTER COLUMN id SET DEFAULT nextval('public."Student_id_seq"'::regclass);


--
-- TOC entry 3009 (class 0 OID 113522)
-- Dependencies: 203
-- Data for Name: AcademicPerformance; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."AcademicPerformance" (id, name, description, code) FROM stdin;
\.


--
-- TOC entry 3007 (class 0 OID 113511)
-- Dependencies: 201
-- Data for Name: Sex; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Sex" (id, name, description, code) FROM stdin;
1	Женский		female
2	Мужской		male
\.


--
-- TOC entry 3011 (class 0 OID 113533)
-- Dependencies: 205
-- Data for Name: Student; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Student" (id, "surName", "firstName", "secondName", dob, "idSex", "idAcademicPerformance") FROM stdin;
\.


--
-- TOC entry 3020 (class 0 OID 0)
-- Dependencies: 202
-- Name: AcademicPerformance_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."AcademicPerformance_id_seq"', 1, false);


--
-- TOC entry 3021 (class 0 OID 0)
-- Dependencies: 200
-- Name: Sex_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Sex_id_seq"', 1, false);


--
-- TOC entry 3022 (class 0 OID 0)
-- Dependencies: 204
-- Name: Student_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Student_id_seq"', 1, false);


--
-- TOC entry 2871 (class 2606 OID 113530)
-- Name: AcademicPerformance AcademicPerformance_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."AcademicPerformance"
    ADD CONSTRAINT "AcademicPerformance_pkey" PRIMARY KEY (id);


--
-- TOC entry 2869 (class 2606 OID 113519)
-- Name: Sex Sex_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Sex"
    ADD CONSTRAINT "Sex_pkey" PRIMARY KEY (id);


--
-- TOC entry 2873 (class 2606 OID 113541)
-- Name: Student Student_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "Student_pkey" PRIMARY KEY (id);


--
-- TOC entry 2874 (class 2606 OID 113542)
-- Name: Student R_254; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "R_254" FOREIGN KEY ("idSex") REFERENCES public."Sex"(id);


--
-- TOC entry 2875 (class 2606 OID 113547)
-- Name: Student R_255; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "R_255" FOREIGN KEY ("idAcademicPerformance") REFERENCES public."AcademicPerformance"(id);


-- Completed on 2021-08-02 21:36:23

--
-- PostgreSQL database dump complete
--

