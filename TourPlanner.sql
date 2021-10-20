--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

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

--
-- Name: add_new_log(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.add_new_log(tour_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
	log_id integer;
BEGIN
   INSERT INTO logs(tourid) VALUES (tour_id);
   SELECT logid into log_id FROM logs ORDER BY logid DESC LIMIT 1;
   RETURN log_id;
END;
$$;


ALTER FUNCTION public.add_new_log(tour_id integer) OWNER TO postgres;

--
-- Name: add_new_tour(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.add_new_tour() RETURNS integer
    LANGUAGE plpgsql
    AS $$
declare
	new_tour_id integer;
BEGIN
   INSERT INTO tours(tourname) VALUES ('New Tour');
   SELECT tourid into new_tour_id FROM tours ORDER BY tourid DESC LIMIT 1;
   RETURN new_tour_id;
END;
$$;


ALTER FUNCTION public.add_new_tour() OWNER TO postgres;

--
-- Name: logid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.logid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.logid_seq OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.logs (
    logid integer DEFAULT nextval('public.logid_seq'::regclass) NOT NULL,
    tourid integer NOT NULL,
    datee timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    durationtime integer DEFAULT 0,
    distance integer DEFAULT 0,
    elevationgain integer DEFAULT 0,
    sleeptime integer DEFAULT 0,
    stepcounter integer DEFAULT 0,
    intakecalories integer DEFAULT 0,
    weather text DEFAULT ' '::text,
    rating text DEFAULT ' '::text,
    notices text DEFAULT ' '::text
);


ALTER TABLE public.logs OWNER TO postgres;

--
-- Name: tourid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tourid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tourid_seq OWNER TO postgres;

--
-- Name: tours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tours (
    tourid integer DEFAULT nextval('public.tourid_seq'::regclass) NOT NULL,
    tourname text DEFAULT ' '::text,
    startp text DEFAULT ' '::text,
    destination text DEFAULT ' '::text,
    tourdescription text DEFAULT ' '::text,
    routeinformation text DEFAULT ' '::text,
    routeimagepath text DEFAULT ' '::text,
    tourdistance real DEFAULT 0,
    fuelused real DEFAULT 0,
    routetype integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.tours OWNER TO postgres;

--
-- Data for Name: logs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.logs (logid, tourid, datee, durationtime, distance, elevationgain, sleeptime, stepcounter, intakecalories, weather, rating, notices) FROM stdin;
1	1	2021-04-09 00:17:01	0	1112	0	0	0	0	 	 	ABCD
2	2	2020-04-09 00:17:01	0	1212	0	0	0	0	 	 	ABCD
3	3	2021-04-28 06:57:01	0	1213	0	0	0	0	 	 	ABCD
4	4	2018-05-16 01:37:01	0	1214	0	0	0	0	 	 	ABCD
5	5	2017-06-02 20:17:01	0	1215	0	0	0	0	 	 	ABCD
9	1	2021-05-05 19:56:13.178618	0	0	0	0	0	0	 	 	 
7	7	2021-05-05 19:50:56.574825	1	0	0	0	0	0	 	 	a
12	7	2021-05-05 20:05:43.886718	0	0	0	5	0	0	Write Like a Lake	 	 
13	7	2021-05-06 12:21:27.24405	0	0	0	0	0	3	 	 	 
\.


--
-- Data for Name: tours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tours (tourid, tourname, startp, destination, tourdescription, routeinformation, routeimagepath, tourdistance, fuelused, routetype) FROM stdin;
7	Best Tour	 Hollabrunn	 Stockerau	 	Directions of Route From  Hollabrunn to  Stockerau\nTour distance: 26,4335km\nTour Time: 00:21:40\n---------------\n1. Fahren Sie zunächst auf die L27/Amtsgasse Richtung Theodor-Körner-Gasse nach Süden.; \n\tDirection distance 0,095km; \n\tTime for Direction 00:00:08\n\n2. Bleiben Sie geradeaus und fahren Sie auf den Kirchenplatz.; \n\tDirection distance 0,1239km; \n\tTime for Direction 00:00:14\n\n3. Der Kirchenplatz wird die Schulgasse.; \n\tDirection distance 0,1642km; \n\tTime for Direction 00:00:26\n\n4. Die Schulgasse wird die Wiener Straße.; \n\tDirection distance 1,2666km; \n\tTime for Direction 00:01:46\n\n5. Abbiegen nach leicht rechts auf die Auffahrt.; \n\tDirection distance 0,0772km; \n\tTime for Direction 00:00:05\n\n6. Halten Sie sich links an der Abzweigung in die Auffahrt.; \n\tDirection distance 0,7628km; \n\tTime for Direction 00:01:15\n\n7. Fädeln Sie sich ein auf die E59 (Teilweise mautpflichtig).; \n\tDirection distance 22,732km; \n\tTime for Direction 00:15:34\n\n8. Nehmen Sie die Ausfahrt in Richtung Stockerau-Mitte.; \n\tDirection distance 0,2945km; \n\tTime for Direction 00:00:21\n\n9. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 1. Ausfahrt.; \n\tDirection distance 0,1625km; \n\tTime for Direction 00:00:21\n\n10. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 2. Ausfahrt auf die In der Au.; \n\tDirection distance 0,4587km; \n\tTime for Direction 00:00:56\n\n11. Abbiegen nach links auf die Donaustraße.; \n\tDirection distance 0,1368km; \n\tTime for Direction 00:00:19\n\n12. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 1. Ausfahrt auf die B3.; \n\tDirection distance 0,1593km; \n\tTime for Direction 00:00:15\n\n13. Willkommen in STOCKERAU, LOWER AUSTRIA.; \n\tDirection distance 0km; \n\tTime for Direction 00:00:00\n\n	C:\\Users\\Raphael\\source\\repos\\Tourplanner\\Tourplanner\\bin\\Debug\\net5.0-windows\\Images\\TourMaps\\rrb0uafw.s3z.png	26.4335	0.73	0
2	Tour 2	Hollabrunn	Salzburg	 	Directions of Route From Hollabrunn to Salzburg\nTour distance: 290,7843km\nTour Time: 03:29:27\n---------------\n1. Fahren Sie zunächst auf die L27/Amtsgasse Richtung Klostergasse nach Norden.; \n\tDirection distance 0,0386km; \n\tTime for Direction 00:00:03\n\n2. Abbiegen nach links auf die Klostergasse.; \n\tDirection distance 0,1062km; \n\tTime for Direction 00:00:19\n\n3. Abbiegen nach leicht links auf den Hauptplatz.; \n\tDirection distance 0,0644km; \n\tTime for Direction 00:00:15\n\n4. Der Hauptplatz wird die Bahnstraße.; \n\tDirection distance 0,3347km; \n\tTime for Direction 00:01:01\n\n5. Abbiegen nach links auf die Parkgasse.; \n\tDirection distance 0,1979km; \n\tTime for Direction 00:00:18\n\n6. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 1. Ausfahrt auf die L27.; \n\tDirection distance 1,0235km; \n\tTime for Direction 00:01:25\n\n7. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 1. Ausfahrt auf die L43.; \n\tDirection distance 3,7031km; \n\tTime for Direction 00:03:53\n\n8. Abbiegen nach links auf die L43/Fahndorfer Straße.; \n\tDirection distance 0,1094km; \n\tTime for Direction 00:00:11\n\n9. Abbiegen nach rechts auf die L43/Fahndorfer Straße. Fahren Sie weiter auf die L43.; \n\tDirection distance 6,2603km; \n\tTime for Direction 00:07:12\n\n10. Nehmen Sie die Auffahrt L48.; \n\tDirection distance 0,0644km; \n\tTime for Direction 00:00:10\n\n11. Bleiben Sie geradeaus und fahren Sie auf die L48.; \n\tDirection distance 3,4054km; \n\tTime for Direction 00:03:43\n\n12. Abbiegen nach links auf die L1228/Minichhofen. Fahren Sie weiter auf die L1228.; \n\tDirection distance 3,93km; \n\tTime for Direction 00:05:27\n\n13. Bleiben Sie geradeaus und fahren Sie auf die L43.; \n\tDirection distance 13,1226km; \n\tTime for Direction 00:14:24\n\n14. Abbiegen nach links auf die B35. Durchfahren Sie die 2 Kreisverkehre.; \n\tDirection distance 14,2202km; \n\tTime for Direction 00:16:14\n\n15. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 3. Ausfahrt auf B35/Franz-Zeller-Platz.; \n\tDirection distance 0,095km; \n\tTime for Direction 00:00:18\n\n16. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 2. Ausfahrt auf die B3.; \n\tDirection distance 0,795km; \n\tTime for Direction 00:00:53\n\n17. Abbiegen nach links auf Steiner Donaulände.; \n\tDirection distance 0,4023km; \n\tTime for Direction 00:01:02\n\n18. Abbiegen nach links auf die B33a.; \n\tDirection distance 0,5263km; \n\tTime for Direction 00:00:33\n\n19. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 1. Ausfahrt auf die L114/Kremser Straße.; \n\tDirection distance 0,2173km; \n\tTime for Direction 00:00:27\n\n20. Abbiegen nach rechts auf die Melker Straße.; \n\tDirection distance 1,7542km; \n\tTime for Direction 00:02:33\n\n21. Die Melker Straße wird die L109.; \n\tDirection distance 10,2419km; \n\tTime for Direction 00:11:45\n\n22. Abbiegen nach rechts auf die L109/Melker Straße.; \n\tDirection distance 3,4054km; \n\tTime for Direction 00:04:15\n\n23. Abbiegen nach rechts und bleiben Sie auf die L109/Melker Straße.; \n\tDirection distance 1,4774km; \n\tTime for Direction 00:01:51\n\n24. Abbiegen nach rechts auf die L162.; \n\tDirection distance 3,2525km; \n\tTime for Direction 00:04:10\n\n25. Abbiegen nach links auf die B33.; \n\tDirection distance 9,3599km; \n\tTime for Direction 00:07:50\n\n26. Abbiegen nach rechts auf die B1/Räcking. Fahren Sie weiter auf die B1.; \n\tDirection distance 9,5354km; \n\tTime for Direction 00:08:15\n\n27. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 4. Ausfahrt auf die L5325.; \n\tDirection distance 0,2865km; \n\tTime for Direction 00:00:55\n\n28. Nehmen Sie die Auffahrt in Richtung Linz.; \n\tDirection distance 0,5021km; \n\tTime for Direction 00:00:28\n\n29. Fädeln Sie sich ein auf die A1/E60 (Teilweise mautpflichtig).; \n\tDirection distance 190,2052km; \n\tTime for Direction 01:31:04\n\n30. Nehmen Sie die Ausfahrt A1, Ausfahrt 281, in Richtung Wallersee/Eugendorf.; \n\tDirection distance 0,2752km; \n\tTime for Direction 00:00:14\n\n31. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 3. Ausfahrt.; \n\tDirection distance 0,4506km; \n\tTime for Direction 00:00:37\n\n32. Fahren Sie in den nächsten Kreisverkehr und nehmen Sie die 2. Ausfahrt auf die B1. Durchfahren Sie den 1 Kreisverkehr.; \n\tDirection distance 7,7699km; \n\tTime for Direction 00:10:20\n\n33. Halten Sie sich links an der Abzweigung, um auf die B1/Linzer Bundesstraße zu fahren.; \n\tDirection distance 0,0644km; \n\tTime for Direction 00:00:20\n\n34. Abbiegen nach links auf die B150/Fürbergstraße. Fahren Sie weiter auf die B150.; \n\tDirection distance 1,3696km; \n\tTime for Direction 00:02:13\n\n35. Abbiegen nach rechts auf die B150/Bürglsteinstraße. Fahren Sie weiter auf die B150.; \n\tDirection distance 0,6695km; \n\tTime for Direction 00:01:04\n\n36. Bleiben Sie geradeaus und fahren Sie auf den Dr.-Franz-Rehrl-Platz.; \n\tDirection distance 0,066km; \n\tTime for Direction 00:00:08\n\n37. Bleiben Sie geradeaus und fahren Sie auf die Imbergstraße.; \n\tDirection distance 0,7451km; \n\tTime for Direction 00:01:18\n\n38. Abbiegen nach links auf Staatsbrücke.; \n\tDirection distance 0,1014km; \n\tTime for Direction 00:00:11\n\n39. Abbiegen nach rechts auf die Griesgasse.; \n\tDirection distance 0,0853km; \n\tTime for Direction 00:00:19\n\n40. Die Griesgasse wird der Ferdinand-Hanusch-Platz.; \n\tDirection distance 0,1062km; \n\tTime for Direction 00:00:16\n\n41. Bleiben Sie geradeaus und fahren Sie auf den Franz-Josef-Kai.; \n\tDirection distance 0,1947km; \n\tTime for Direction 00:00:40\n\n42. Abbiegen nach links und bleiben Sie auf den Franz-Josef-Kai.; \n\tDirection distance 0,0113km; \n\tTime for Direction 00:00:07\n\n43. Abbiegen nach leicht links auf den Museumsplatz.; \n\tDirection distance 0,0772km; \n\tTime for Direction 00:00:11\n\n44. Der Museumsplatz wird der Anton-Neumayr-Platz.; \n\tDirection distance 0,0483km; \n\tTime for Direction 00:00:11\n\n45. Der Anton-Neumayr-Platz wird die Gstättengasse.; \n\tDirection distance 0,0515km; \n\tTime for Direction 00:00:11\n\n46. Die Gstättengasse wird Gstättentor (Schleifer-Bogen).; \n\tDirection distance 0,008km; \n\tTime for Direction 00:00:02\n\n47. Gstättentor (Schleifer-Bogen) wird der Bürgerspitalplatz.; \n\tDirection distance 0,0435km; \n\tTime for Direction 00:00:10\n\n48. Abbiegen nach rechts und bleiben Sie auf den Bürgerspitalplatz.; \n\tDirection distance 0,0097km; \n\tTime for Direction 00:00:01\n\n49. Willkommen in SALZBURG, SALZBURG.; \n\tDirection distance 0km; \n\tTime for Direction 00:00:00\n\n	C:\\Users\\Raphael\\source\\repos\\Tourplanner\\Tourplanner\\bin\\Debug\\net5.0-windows\\Images\\TourMaps\\wwc3shf3.fzf.png	290.7843	9.9	1
3	Tour 3	Hollabrunn	Vorarlberg	 	 	 	0	0	2
4	Apfel	Hollabrunn	Grado	 	 	 	0	0	3
5	Appel	Hollabrunn	Korneuburg	 	Directions of Route From Hollabrunn to Korneuburg\nTour distance: 39,0314km\nTour Time: 00:28:12\n---------------\n1. Fahren Sie zunächst auf die L27/Amtsgasse Richtung Theodor-Körner-Gasse nach Süden.; \n\tDirection distance 0,095km; \n\tTime for Direction 00:00:08\n\n2. Bleiben Sie geradeaus und fahren Sie auf den Kirchenplatz.; \n\tDirection distance 0,1239km; \n\tTime for Direction 00:00:14\n\n3. Der Kirchenplatz wird die Schulgasse.; \n\tDirection distance 0,1642km; \n\tTime for Direction 00:00:26\n\n4. Die Schulgasse wird die Wiener Straße.; \n\tDirection distance 1,2666km; \n\tTime for Direction 00:01:46\n\n5. Abbiegen nach leicht rechts auf die Auffahrt.; \n\tDirection distance 0,0772km; \n\tTime for Direction 00:00:05\n\n6. Halten Sie sich links an der Abzweigung in die Auffahrt.; \n\tDirection distance 0,7628km; \n\tTime for Direction 00:01:15\n\n7. Fädeln Sie sich ein auf die E59 (Teilweise mautpflichtig).; \n\tDirection distance 34,3305km; \n\tTime for Direction 00:21:19\n\n8. Nehmen Sie Ausfahrt 16 in Richtung Korneuburg.; \n\tDirection distance 1,2553km; \n\tTime for Direction 00:01:29\n\n9. Abbiegen nach links auf die B3/Wiener Straße.; \n\tDirection distance 0,956km; \n\tTime for Direction 00:01:30\n\n10. Willkommen in KORNEUBURG, LOWER AUSTRIA.; \n\tDirection distance 0km; \n\tTime for Direction 00:00:00\n\n	C:\\Users\\Raphael\\source\\repos\\Tourplanner\\Tourplanner\\bin\\Debug\\net5.0-windows\\Images\\TourMaps\\ilviyo3y.itm.png	39.0314	1.15	0
1	Tour 1	Hollabrunn	Wien	 	Directions of Route From Hollabrunn to Wien\nTour distance: 56,1194km\nTour Time: 00:46:05\n---------------\n1. Fahren Sie zunächst auf die L27/Amtsgasse Richtung Theodor-Körner-Gasse nach Süden.; \n\tDirection distance 0,095km; \n\tTime for Direction 00:00:08\n\n2. Bleiben Sie geradeaus und fahren Sie auf den Kirchenplatz.; \n\tDirection distance 0,1239km; \n\tTime for Direction 00:00:14\n\n3. Der Kirchenplatz wird die Schulgasse.; \n\tDirection distance 0,1642km; \n\tTime for Direction 00:00:26\n\n4. Die Schulgasse wird die Wiener Straße.; \n\tDirection distance 1,2666km; \n\tTime for Direction 00:01:46\n\n5. Abbiegen nach leicht rechts auf die Auffahrt.; \n\tDirection distance 0,0772km; \n\tTime for Direction 00:00:05\n\n6. Halten Sie sich links an der Abzweigung in die Auffahrt.; \n\tDirection distance 0,7628km; \n\tTime for Direction 00:01:15\n\n7. Fädeln Sie sich ein auf die E59 (Teilweise mautpflichtig).; \n\tDirection distance 42,6605km; \n\tTime for Direction 00:26:32\n\n8. Nehmen Sie Ausfahrt 7 in Richtung Zentrum/Gürtel/Nordbrücke.; \n\tDirection distance 1,4629km; \n\tTime for Direction 00:01:17\n\n9. Halten Sie sich links an der Abzweigung in die Auffahrt.; \n\tDirection distance 0,1255km; \n\tTime for Direction 00:00:08\n\n10. Halten Sie sich links an der Abzweigung in die Auffahrt.; \n\tDirection distance 0,0322km; \n\tTime for Direction 00:00:02\n\n11. Fahren Sie geradeaus, um auf die Auffahrt A22 in Richtung Gürtel/Zentrum/Döbling zu fahren.; \n\tDirection distance 0,111km; \n\tTime for Direction 00:00:06\n\n12. Fahren Sie geradeaus, um auf die Auffahrt A22 zu fahren.; \n\tDirection distance 0,2655km; \n\tTime for Direction 00:00:14\n\n13. Fahren Sie geradeaus, um auf die Auffahrt A22 zu fahren.; \n\tDirection distance 0,1046km; \n\tTime for Direction 00:00:09\n\n14. Fädeln Sie sich ein auf die B227.; \n\tDirection distance 5,3044km; \n\tTime for Direction 00:07:20\n\n15. Abbiegen nach leicht rechts auf die B1/Radetzkybrücke. Fahren Sie weiter auf die B1.; \n\tDirection distance 1,1925km; \n\tTime for Direction 00:01:48\n\n16. Abbiegen nach rechts auf die B1/Johannesgasse.; \n\tDirection distance 0,1191km; \n\tTime for Direction 00:00:21\n\n17. Abbiegen nach links auf die B1/Lothringerstraße. Fahren Sie weiter auf die B1.; \n\tDirection distance 1,001km; \n\tTime for Direction 00:01:44\n\n18. Abbiegen nach leicht links auf die Operngasse.; \n\tDirection distance 0,4587km; \n\tTime for Direction 00:00:57\n\n19. Abbiegen nach leicht rechts auf die Margaretenstraße.; \n\tDirection distance 0,0724km; \n\tTime for Direction 00:00:08\n\n20. Abbiegen nach links auf die Paulanergasse.; \n\tDirection distance 0,2849km; \n\tTime for Direction 00:00:35\n\n21. Die Paulanergasse wird die Favoritenstraße.; \n\tDirection distance 0,4345km; \n\tTime for Direction 00:00:50\n\n22. Willkommen in VIENNA, VIENNA.; \n\tDirection distance 0km; \n\tTime for Direction 00:00:00\n\n	C:\\Users\\Raphael\\source\\repos\\Tourplanner\\Tourplanner\\bin\\Debug\\net5.0-windows\\Images\\TourMaps\\1c5jmdjn.02q.png	56.1194	1.73	0
78	New Tour	 	 	 	 	 	0	0	0
\.


--
-- Name: logid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.logid_seq', 59, true);


--
-- Name: tourid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tourid_seq', 78, true);


--
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (logid);


--
-- Name: tours tours_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tours
    ADD CONSTRAINT tours_pkey PRIMARY KEY (tourid);


--
-- Name: logs tourid_fk; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT tourid_fk FOREIGN KEY (tourid) REFERENCES public.tours(tourid);


--
-- PostgreSQL database dump complete
--

