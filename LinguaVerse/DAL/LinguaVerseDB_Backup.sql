PGDMP  /    
        
        |           LinguaVerseDB    16.3    16.3 9    '           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            (           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            )           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            *           1262    16397    LinguaVerseDB    DATABASE     �   CREATE DATABASE "LinguaVerseDB" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Italian_Italy.1252';
    DROP DATABASE "LinguaVerseDB";
                postgres    false            �            1259    24589    CourseProgress    TABLE     �   CREATE TABLE public."CourseProgress" (
    progressid integer NOT NULL,
    userid integer NOT NULL,
    coursename character varying(50) NOT NULL,
    progress double precision,
    level character varying(50)
);
 $   DROP TABLE public."CourseProgress";
       public         heap    postgres    false            �            1259    24588    CourseProgress_progressid_seq    SEQUENCE     �   CREATE SEQUENCE public."CourseProgress_progressid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 6   DROP SEQUENCE public."CourseProgress_progressid_seq";
       public          postgres    false    220            +           0    0    CourseProgress_progressid_seq    SEQUENCE OWNED BY     c   ALTER SEQUENCE public."CourseProgress_progressid_seq" OWNED BY public."CourseProgress".progressid;
          public          postgres    false    219            �            1259    24577    DailyStreaks    TABLE     �   CREATE TABLE public."DailyStreaks" (
    streakid integer NOT NULL,
    userid integer NOT NULL,
    day character varying(10) NOT NULL
);
 "   DROP TABLE public."DailyStreaks";
       public         heap    postgres    false            �            1259    24576    DailyStreaks_streakid_seq    SEQUENCE     �   CREATE SEQUENCE public."DailyStreaks_streakid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 2   DROP SEQUENCE public."DailyStreaks_streakid_seq";
       public          postgres    false    218            ,           0    0    DailyStreaks_streakid_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public."DailyStreaks_streakid_seq" OWNED BY public."DailyStreaks".streakid;
          public          postgres    false    217            �            1259    24601    FeaturedCourses    TABLE     �   CREATE TABLE public."FeaturedCourses" (
    courseid integer NOT NULL,
    coursename character varying(50) NOT NULL,
    duration integer,
    questions integer,
    level character varying(50),
    "flagIcon" character varying(255)
);
 %   DROP TABLE public."FeaturedCourses";
       public         heap    postgres    false            �            1259    24600    FeaturedCourses_courseid_seq    SEQUENCE     �   CREATE SEQUENCE public."FeaturedCourses_courseid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public."FeaturedCourses_courseid_seq";
       public          postgres    false    222            -           0    0    FeaturedCourses_courseid_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."FeaturedCourses_courseid_seq" OWNED BY public."FeaturedCourses".courseid;
          public          postgres    false    221            �            1259    32841 	   Questions    TABLE     �   CREATE TABLE public."Questions" (
    "QuestionID" integer NOT NULL,
    "QuizID" integer NOT NULL,
    "QuestionText" text,
    "Answer" character varying(100),
    "Choices" text[]
);
    DROP TABLE public."Questions";
       public         heap    postgres    false            �            1259    32840    Questions_QuestionID_seq    SEQUENCE     �   CREATE SEQUENCE public."Questions_QuestionID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 1   DROP SEQUENCE public."Questions_QuestionID_seq";
       public          postgres    false    228            .           0    0    Questions_QuestionID_seq    SEQUENCE OWNED BY     [   ALTER SEQUENCE public."Questions_QuestionID_seq" OWNED BY public."Questions"."QuestionID";
          public          postgres    false    227            �            1259    32834    Quizzes    TABLE     �   CREATE TABLE public."Quizzes" (
    "QuizID" integer NOT NULL,
    "Title" character varying(100),
    "Category" character varying(50),
    "Level" character varying(50)
);
    DROP TABLE public."Quizzes";
       public         heap    postgres    false            �            1259    32833    Quizzes_QuizID_seq    SEQUENCE     �   CREATE SEQUENCE public."Quizzes_QuizID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 +   DROP SEQUENCE public."Quizzes_QuizID_seq";
       public          postgres    false    226            /           0    0    Quizzes_QuizID_seq    SEQUENCE OWNED BY     O   ALTER SEQUENCE public."Quizzes_QuizID_seq" OWNED BY public."Quizzes"."QuizID";
          public          postgres    false    225            �            1259    16399    User    TABLE     �   CREATE TABLE public."User" (
    userid integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(255) NOT NULL,
    languagepreference character varying(50),
    progress double precision
);
    DROP TABLE public."User";
       public         heap    postgres    false            �            1259    32803    UserProgress    TABLE     �   CREATE TABLE public."UserProgress" (
    "UserProgressID" integer NOT NULL,
    "UserID" integer NOT NULL,
    "QuizID" integer NOT NULL,
    "Score" integer,
    "CompletionTime" integer,
    "AttemptDate" timestamp without time zone
);
 "   DROP TABLE public."UserProgress";
       public         heap    postgres    false            �            1259    32802    UserProgress_UserProgressID_seq    SEQUENCE     �   CREATE SEQUENCE public."UserProgress_UserProgressID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 8   DROP SEQUENCE public."UserProgress_UserProgressID_seq";
       public          postgres    false    224            0           0    0    UserProgress_UserProgressID_seq    SEQUENCE OWNED BY     i   ALTER SEQUENCE public."UserProgress_UserProgressID_seq" OWNED BY public."UserProgress"."UserProgressID";
          public          postgres    false    223            �            1259    16398    User_userid_seq    SEQUENCE     �   CREATE SEQUENCE public."User_userid_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."User_userid_seq";
       public          postgres    false    216            1           0    0    User_userid_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."User_userid_seq" OWNED BY public."User".userid;
          public          postgres    false    215            p           2604    24592    CourseProgress progressid    DEFAULT     �   ALTER TABLE ONLY public."CourseProgress" ALTER COLUMN progressid SET DEFAULT nextval('public."CourseProgress_progressid_seq"'::regclass);
 J   ALTER TABLE public."CourseProgress" ALTER COLUMN progressid DROP DEFAULT;
       public          postgres    false    219    220    220            o           2604    24580    DailyStreaks streakid    DEFAULT     �   ALTER TABLE ONLY public."DailyStreaks" ALTER COLUMN streakid SET DEFAULT nextval('public."DailyStreaks_streakid_seq"'::regclass);
 F   ALTER TABLE public."DailyStreaks" ALTER COLUMN streakid DROP DEFAULT;
       public          postgres    false    217    218    218            q           2604    24604    FeaturedCourses courseid    DEFAULT     �   ALTER TABLE ONLY public."FeaturedCourses" ALTER COLUMN courseid SET DEFAULT nextval('public."FeaturedCourses_courseid_seq"'::regclass);
 I   ALTER TABLE public."FeaturedCourses" ALTER COLUMN courseid DROP DEFAULT;
       public          postgres    false    221    222    222            t           2604    32844    Questions QuestionID    DEFAULT     �   ALTER TABLE ONLY public."Questions" ALTER COLUMN "QuestionID" SET DEFAULT nextval('public."Questions_QuestionID_seq"'::regclass);
 G   ALTER TABLE public."Questions" ALTER COLUMN "QuestionID" DROP DEFAULT;
       public          postgres    false    227    228    228            s           2604    32837    Quizzes QuizID    DEFAULT     v   ALTER TABLE ONLY public."Quizzes" ALTER COLUMN "QuizID" SET DEFAULT nextval('public."Quizzes_QuizID_seq"'::regclass);
 A   ALTER TABLE public."Quizzes" ALTER COLUMN "QuizID" DROP DEFAULT;
       public          postgres    false    226    225    226            n           2604    16402    User userid    DEFAULT     n   ALTER TABLE ONLY public."User" ALTER COLUMN userid SET DEFAULT nextval('public."User_userid_seq"'::regclass);
 <   ALTER TABLE public."User" ALTER COLUMN userid DROP DEFAULT;
       public          postgres    false    216    215    216            r           2604    32806    UserProgress UserProgressID    DEFAULT     �   ALTER TABLE ONLY public."UserProgress" ALTER COLUMN "UserProgressID" SET DEFAULT nextval('public."UserProgress_UserProgressID_seq"'::regclass);
 N   ALTER TABLE public."UserProgress" ALTER COLUMN "UserProgressID" DROP DEFAULT;
       public          postgres    false    223    224    224                      0    24589    CourseProgress 
   TABLE DATA           [   COPY public."CourseProgress" (progressid, userid, coursename, progress, level) FROM stdin;
    public          postgres    false    220   �C                 0    24577    DailyStreaks 
   TABLE DATA           ?   COPY public."DailyStreaks" (streakid, userid, day) FROM stdin;
    public          postgres    false    218   D                 0    24601    FeaturedCourses 
   TABLE DATA           i   COPY public."FeaturedCourses" (courseid, coursename, duration, questions, level, "flagIcon") FROM stdin;
    public          postgres    false    222   D       $          0    32841 	   Questions 
   TABLE DATA           b   COPY public."Questions" ("QuestionID", "QuizID", "QuestionText", "Answer", "Choices") FROM stdin;
    public          postgres    false    228   ;D       "          0    32834    Quizzes 
   TABLE DATA           K   COPY public."Quizzes" ("QuizID", "Title", "Category", "Level") FROM stdin;
    public          postgres    false    226   �E                 0    16399    User 
   TABLE DATA           Z   COPY public."User" (userid, username, password, languagepreference, progress) FROM stdin;
    public          postgres    false    216   lF                  0    32803    UserProgress 
   TABLE DATA           x   COPY public."UserProgress" ("UserProgressID", "UserID", "QuizID", "Score", "CompletionTime", "AttemptDate") FROM stdin;
    public          postgres    false    224   �F       2           0    0    CourseProgress_progressid_seq    SEQUENCE SET     N   SELECT pg_catalog.setval('public."CourseProgress_progressid_seq"', 1, false);
          public          postgres    false    219            3           0    0    DailyStreaks_streakid_seq    SEQUENCE SET     J   SELECT pg_catalog.setval('public."DailyStreaks_streakid_seq"', 1, false);
          public          postgres    false    217            4           0    0    FeaturedCourses_courseid_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."FeaturedCourses_courseid_seq"', 1, false);
          public          postgres    false    221            5           0    0    Questions_QuestionID_seq    SEQUENCE SET     I   SELECT pg_catalog.setval('public."Questions_QuestionID_seq"', 14, true);
          public          postgres    false    227            6           0    0    Quizzes_QuizID_seq    SEQUENCE SET     C   SELECT pg_catalog.setval('public."Quizzes_QuizID_seq"', 10, true);
          public          postgres    false    225            7           0    0    UserProgress_UserProgressID_seq    SEQUENCE SET     P   SELECT pg_catalog.setval('public."UserProgress_UserProgressID_seq"', 1, false);
          public          postgres    false    223            8           0    0    User_userid_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."User_userid_seq"', 5, true);
          public          postgres    false    215            |           2606    24594 "   CourseProgress CourseProgress_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public."CourseProgress"
    ADD CONSTRAINT "CourseProgress_pkey" PRIMARY KEY (progressid);
 P   ALTER TABLE ONLY public."CourseProgress" DROP CONSTRAINT "CourseProgress_pkey";
       public            postgres    false    220            z           2606    24582    DailyStreaks DailyStreaks_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public."DailyStreaks"
    ADD CONSTRAINT "DailyStreaks_pkey" PRIMARY KEY (streakid);
 L   ALTER TABLE ONLY public."DailyStreaks" DROP CONSTRAINT "DailyStreaks_pkey";
       public            postgres    false    218            ~           2606    24606 $   FeaturedCourses FeaturedCourses_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public."FeaturedCourses"
    ADD CONSTRAINT "FeaturedCourses_pkey" PRIMARY KEY (courseid);
 R   ALTER TABLE ONLY public."FeaturedCourses" DROP CONSTRAINT "FeaturedCourses_pkey";
       public            postgres    false    222            �           2606    32848    Questions Questions_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public."Questions"
    ADD CONSTRAINT "Questions_pkey" PRIMARY KEY ("QuestionID");
 F   ALTER TABLE ONLY public."Questions" DROP CONSTRAINT "Questions_pkey";
       public            postgres    false    228            �           2606    32839    Quizzes Quizzes_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public."Quizzes"
    ADD CONSTRAINT "Quizzes_pkey" PRIMARY KEY ("QuizID");
 B   ALTER TABLE ONLY public."Quizzes" DROP CONSTRAINT "Quizzes_pkey";
       public            postgres    false    226            �           2606    32808    UserProgress UserProgress_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public."UserProgress"
    ADD CONSTRAINT "UserProgress_pkey" PRIMARY KEY ("UserProgressID");
 L   ALTER TABLE ONLY public."UserProgress" DROP CONSTRAINT "UserProgress_pkey";
       public            postgres    false    224            v           2606    16404    User User_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY (userid);
 <   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_pkey";
       public            postgres    false    216            x           2606    16406    User User_username_key 
   CONSTRAINT     Y   ALTER TABLE ONLY public."User"
    ADD CONSTRAINT "User_username_key" UNIQUE (username);
 D   ALTER TABLE ONLY public."User" DROP CONSTRAINT "User_username_key";
       public            postgres    false    216            �           2606    24595 )   CourseProgress CourseProgress_userid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."CourseProgress"
    ADD CONSTRAINT "CourseProgress_userid_fkey" FOREIGN KEY (userid) REFERENCES public."User"(userid);
 W   ALTER TABLE ONLY public."CourseProgress" DROP CONSTRAINT "CourseProgress_userid_fkey";
       public          postgres    false    4726    220    216            �           2606    24583 %   DailyStreaks DailyStreaks_userid_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."DailyStreaks"
    ADD CONSTRAINT "DailyStreaks_userid_fkey" FOREIGN KEY (userid) REFERENCES public."User"(userid);
 S   ALTER TABLE ONLY public."DailyStreaks" DROP CONSTRAINT "DailyStreaks_userid_fkey";
       public          postgres    false    218    4726    216            �           2606    32849    Questions Questions_QuizID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Questions"
    ADD CONSTRAINT "Questions_QuizID_fkey" FOREIGN KEY ("QuizID") REFERENCES public."Quizzes"("QuizID");
 M   ALTER TABLE ONLY public."Questions" DROP CONSTRAINT "Questions_QuizID_fkey";
       public          postgres    false    228    4738    226                  x������ � �            x������ � �            x������ � �      $   �  x��R�nA>�>�5.#Ħ��T� u�T)H7���l�hfv���.}�����zB�������W��T��Z`-=D\�����{8M�1�#�Q̘ww,�J��*��U���b�Y�[L�RK��`%��F�-�Ϲ�=2'�2�Q�ctg��j �_o���\'"�Śv�MC���Ӛ�7��i��9go���uw��+����{�S@;L,�;��^�hp��=ؙ�6���'�� }�/n�'�9%r��-����>Rܭ1�^��}�n���ޡכ⅙(X^�ZFۈҿb��#�������f֗?;�)�u�ŋ��N���:ȗZa۩���jhQ�&�swC �%�KƤ*��奒;֙���a�n��%�Ճh���k�|�Hb,��Wc9�k9��\����䧧EQ� �绮      "   l   x�3�tJ,�LV�,I��L�S�(J,N-��I�K/MLO�tJM���K-�2�tL)K�KNM�+v/J��M,B(���2&�XR�5%�X3R�5'�XR��$�XCR̍���� Dy��         /   x�3�J,J-�LL�����".Sΐ����Ԣ���T�=... �             x������ � �     