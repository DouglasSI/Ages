

CREATE TABLE tb_anexo (
    id INTEGER NOT NULL,
    id_usuario INTEGER NOT NULL,
    titulo VARCHAR(100) NOT NULL,
    anotacao VARCHAR(1000),
    data_cadastro DATETIME DEFAULT now() NOT NULL,
    tamanho INTEGER DEFAULT 0 NOT NULL,
    tipo VARCHAR(100)  NOT NULL,
    caminho VARCHAR(1000)  NOT NULL,
    nome_arquivo VARCHAR(1000),
    url VARCHAR(1000)
);


CREATE TABLE tb_anexo_compra (
    id_anexo INTEGER NOT NULL,
    id_compra INTEGER NOT NULL
);

CREATE TABLE tb_anexo_empresa (
    id_anexo INTEGER NOT NULL,
    id_empresa INTEGER NOT NULL
);
CREATE TABLE tb_anexo_fatura (
    id_anexo INTEGER NOT NULL,
    id_fatura INTEGER NOT NULL
);

CREATE TABLE tb_anexo_orcamento (
    id_anexo INTEGER NOT NULL,
    id_orcamento INTEGER NOT NULL
);

CREATE TABLE tb_anexo_projeto (
    id_anexo INTEGER NOT NULL,
    id_projeto INTEGER NOT NULL
);

CREATE TABLE tb_campi (
    id INTEGER NOT NULL ,
    id_endereco INTEGER NOT NULL,
    id_contato INTEGER NOT NULL,
    id_mantenedora INTEGER NOT NULL,
    num_inscricao VARCHAR(20) NOT NULL,
    razao_social VARCHAR(75) NOT NULL,
    nome_fantasia VARCHAR(75) NOT NULL,
    atividade_principal VARCHAR(75) NOT NULL,
    atividade_secundaria VARCHAR(75) NOT NULL,
    natureza_juridica VARCHAR(75) NOT NULL,
    cnpj VARCHAR(20) NOT NULL,
    inscricao_estadual VARCHAR(20) NOT NULL,
    inscricao_municipal VARCHAR(20) NOT NULL
);

CREATE TABLE tb_compra (
    id INTEGER NOT NULL ,
    id_fatura INTEGER NOT NULL,
    anotacao VARCHAR(1000),
    data_compra DATETIME,
    valor numeric(10,4) NOT NULL
);

CREATE TABLE tb_contato (
    id INTEGER NOT NULL ,
    telefone VARCHAR(20) NOT NULL,
    fax VARCHAR(20),
    celular VARCHAR(20) NOT NULL,
    email VARCHAR(60) NOT NULL,
    site VARCHAR(40)
);

CREATE TABLE tb_empresa (
    id INTEGER NOT NULL ,
    id_endereco INTEGER NOT NULL,
    id_contato INTEGER NOT NULL,
    razao_social VARCHAR(75) NOT NULL,
    nome_fantasia VARCHAR(75) NOT NULL,
    cnpj VARCHAR(20) NOT NULL,
    inscricao_estadual VARCHAR(20) NOT NULL,
    inscricao_municipal VARCHAR(20) NOT NULL,
    atividade_principal VARCHAR(100) NOT NULL
);

CREATE TABLE tb_endereco (
    id INTEGER NOT NULL ,
    logradouro VARCHAR(100) NOT NULL,
    numero VARCHAR(10) NOT NULL,
    complemento VARCHAR(100),
    cep VARCHAR(10) NOT NULL,
    bairro VARCHAR(100) NOT NULL,
    municipio VARCHAR(100) NOT NULL,
    uf VARCHAR(2) NOT NULL
);

CREATE TABLE tb_fatura (
    id INTEGER NOT NULL ,
    id_usuario INTEGER NOT NULL,
    id_forma_pagamento INTEGER NOT NULL,
    id_orcamento INTEGER NOT NULL,
    data_pagamento DATETIME,
    data_prevista DATETIME NOT NULL,
    titulo VARCHAR(100) NOT NULL,
    anotacao VARCHAR(1000),
    banco VARCHAR(50),
    agencia VARCHAR(10),
    conta VARCHAR(20),
    data_cadastro DATETIME DEFAULT now() NOT NULL,
    valor_inicial numeric(10,4) NOT NULL,
    valor_pendente numeric(10,4) NOT NULL,
    is_aditivo boolean DEFAULT false NOT NULL,
    autorizado boolean DEFAULT false NOT NULL
);

CREATE TABLE tb_forma_pagamento (
    id INTEGER NOT NULL ,
    descricao VARCHAR(75) NOT NULL,
    ativo boolean DEFAULT true NOT NULL,
    banco VARCHAR(50),
    agencia VARCHAR(20),
    conta_corrente VARCHAR(20),
    digito VARCHAR(20)
);

CREATE TABLE tb_funcao (
    id INTEGER NOT NULL ,
    nome VARCHAR(50) NOT NULL
   
);

CREATE TABLE tb_mantenedora (
    id INTEGER NOT NULL ,
    id_endereco INTEGER NOT NULL,
    id_contato INTEGER NOT NULL,
    num_inscricao VARCHAR(20) NOT NULL,
    razao_social VARCHAR(75) NOT NULL,
    nome_fantasia VARCHAR(75) NOT NULL,
    atividade_principal VARCHAR(75) NOT NULL,
    atividade_secundaria VARCHAR(75) NOT NULL,
    cnpj VARCHAR(20) NOT NULL,
    inscricao_estadual VARCHAR(20) NOT NULL,
    inscricao_municipal VARCHAR(20) NOT NULL
);

CREATE TABLE tb_orcamento (
    id INTEGER NOT NULL ,
    id_usuario INTEGER NOT NULL,
    id_status INTEGER NOT NULL,
    id_empresa INTEGER NOT NULL,
    id_projeto INTEGER NOT NULL,
    valor numeric(10,4) NOT NULL,
    anotacao VARCHAR(500)
);


CREATE TABLE tb_orcamento_servico (
    id_orcamento INTEGER NOT NULL,
    id_servico INTEGER NOT NULL,
    data_cadastro DATETIME DEFAULT now() NOT NULL,
    valor numeric(10,4) NOT NULL,
    anotacao VARCHAR(500)
);

CREATE TABLE tb_projeto (
    id INTEGER NOT NULL ,
    id_usuario INTEGER NOT NULL,
    id_status INTEGER NOT NULL,
    id_campi INTEGER NOT NULL,
    titulo VARCHAR(100) NOT NULL,
    anotacao text,
    valor_estimado numeric(10,4) NOT NULL
);


CREATE TABLE tb_projeto_servico (
    id_projeto INTEGER NOT NULL,
    id_servico INTEGER NOT NULL,
    data_cadastro DATETIME DEFAULT now() NOT NULL,
    valor_limite numeric(10,4) NOT NULL,
    anotacao VARCHAR(500)
);



CREATE TABLE tb_servico (
    id INTEGER NOT NULL ,
    id_usuario INTEGER NOT NULL,
    valor numeric(10,4) NOT NULL,
    titulo VARCHAR(100) NOT NULL,
    anotacao VARCHAR(1000) NOT NULL,
    data_cadastro DATETIME DEFAULT now() NOT NULL
);



CREATE TABLE tb_status_orcamento (
    id INTEGER NOT NULL ,
    descricao VARCHAR(20) NOT NULL
);


CREATE TABLE tb_status_projeto (
    id INTEGER NOT NULL ,
    descricao VARCHAR(20) NOT NULL
);



CREATE TABLE tb_usuario (
    id INTEGER NOT NULL ,
    nome VARCHAR(100) NOT NULL,
    senha VARCHAR(1000) NOT NULL,
    id_funcao INTEGER NOT NULL,
    email VARCHAR(100) NOT NULL,
    ativo boolean DEFAULT true NOT NULL,
    perfil VARCHAR(20) NOT NULL,
    sobrenome VARCHAR(100) NOT NULL
);




ALTER TABLE  tb_anexo_compra
    ADD CONSTRAINT pk_ac PRIMARY KEY (id_anexo, id_compra);


--
-- TOC entry 2119 (class 2606 OID 42140)
-- Name: pk_ae; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_anexo_empresa
    ADD CONSTRAINT pk_ae PRIMARY KEY (id_anexo, id_empresa);


--
-- TOC entry 2113 (class 2606 OID 42095)
-- Name: pk_af; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_anexo_fatura
    ADD CONSTRAINT pk_af PRIMARY KEY (id_anexo, id_fatura);


--
-- TOC entry 2117 (class 2606 OID 42125)
-- Name: pk_ao; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_anexo_orcamento
    ADD CONSTRAINT pk_ao PRIMARY KEY (id_anexo, id_orcamento);


--
-- TOC entry 2115 (class 2606 OID 42110)
-- Name: pk_ap; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_anexo_projeto
    ADD CONSTRAINT pk_ap PRIMARY KEY (id_anexo, id_projeto);


--
-- TOC entry 2103 (class 2606 OID 31735)
-- Name: pk_os_orcamento; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_orcamento_servico
    ADD CONSTRAINT pk_os_orcamento PRIMARY KEY (id_orcamento, id_servico);


--
-- TOC entry 2101 (class 2606 OID 31716)
-- Name: pk_ps_projeto; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_projeto_servico
    ADD CONSTRAINT pk_ps_projeto PRIMARY KEY (id_projeto, id_servico);


--
-- TOC entry 2079 (class 2606 OID 31537)
-- Name: tb_anexo_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_anexo
    ADD CONSTRAINT tb_anexo_pkey PRIMARY KEY (id);
    ALTER TABLE tb_anexo MODIFY COLUMN id INTEGER auto_increment;


--
-- TOC entry 2091 (class 2606 OID 31600)
-- Name: tb_campi_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_campi
    ADD CONSTRAINT tb_campi_pkey PRIMARY KEY (id);
ALTER TABLE tb_campi MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2109 (class 2606 OID 31792)
-- Name: tb_compra_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_compra
    ADD CONSTRAINT tb_compra_pkey PRIMARY KEY (id);
ALTER TABLE tb_compra MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2087 (class 2606 OID 31574)
-- Name: tb_contato_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_contato
    ADD CONSTRAINT tb_contato_pkey PRIMARY KEY (id);
    ALTER TABLE tb_contato MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2093 (class 2606 OID 31623)
-- Name: tb_empresa_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_empresa
    ADD CONSTRAINT tb_empresa_pkey PRIMARY KEY (id);
    ALTER TABLE tb_empresa MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2085 (class 2606 OID 31566)
-- Name: tb_endereco_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_endereco
    ADD CONSTRAINT tb_endereco_pkey PRIMARY KEY (id);
   ALTER TABLE tb_endereco MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2107 (class 2606 OID 31766)
-- Name: tb_fatura_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_fatura
    ADD CONSTRAINT tb_fatura_pkey PRIMARY KEY (id);
   ALTER TABLE tb_fatura MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2105 (class 2606 OID 31753)
-- Name: tb_forma_pagamento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_forma_pagamento
    ADD CONSTRAINT tb_forma_pagamento_pkey PRIMARY KEY (id);
    ALTER TABLE tb_forma_pagamento MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2075 (class 2606 OID 31509)
-- Name: tb_funcao_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_funcao
    ADD CONSTRAINT tb_funcao_pkey PRIMARY KEY (id);
    ALTER TABLE tb_funcao MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2089 (class 2606 OID 31582)
-- Name: tb_mantenedora_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_mantenedora
    ADD CONSTRAINT tb_mantenedora_pkey PRIMARY KEY (id);
    ALTER TABLE tb_mantenedora MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2097 (class 2606 OID 31670)
-- Name: tb_orcamento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_orcamento
    ADD CONSTRAINT tb_orcamento_pkey PRIMARY KEY (id);
    ALTER TABLE tb_orcamento MODIFY COLUMN id INTEGER auto_increment;


--
-- TOC entry 2095 (class 2606 OID 31644)
-- Name: tb_projeto_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_projeto
    ADD CONSTRAINT tb_projeto_pkey PRIMARY KEY (id);
   ALTER TABLE tb_projeto MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2099 (class 2606 OID 31702)
-- Name: tb_servico_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_servico
    ADD CONSTRAINT tb_servico_pkey PRIMARY KEY (id);
    ALTER TABLE tb_servico MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2083 (class 2606 OID 31558)
-- Name: tb_status_orcamento_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_status_orcamento
    ADD CONSTRAINT tb_status_orcamento_pkey PRIMARY KEY (id);
   ALTER TABLE tb_status_orcamento MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2081 (class 2606 OID 31550)
-- Name: tb_status_projeto_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_status_projeto
    ADD CONSTRAINT tb_status_projeto_pkey PRIMARY KEY (id);
   ALTER TABLE tb_status_projeto MODIFY COLUMN id INTEGER auto_increment;

--
-- TOC entry 2077 (class 2606 OID 31520)
-- Name: tb_usuario_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE  tb_usuario
    ADD CONSTRAINT tb_usuario_pkey PRIMARY KEY (id);
    ALTER TABLE tb_usuario MODIFY COLUMN id INTEGER auto_increment;


--
-- TOC entry 2145 (class 2606 OID 33941)
-- Name: fk_ac_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_compra
    ADD CONSTRAINT fk_ac_1 FOREIGN KEY (id_anexo) REFERENCES tb_anexo(id);


--
-- TOC entry 2146 (class 2606 OID 33946)
-- Name: fk_ac_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_compra
    ADD CONSTRAINT fk_ac_2 FOREIGN KEY (id_compra) REFERENCES tb_compra(id);


--
-- TOC entry 2153 (class 2606 OID 42141)
-- Name: fk_ae_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_empresa
    ADD CONSTRAINT fk_ae_1 FOREIGN KEY (id_anexo) REFERENCES tb_anexo(id);


--
-- TOC entry 2154 (class 2606 OID 42146)
-- Name: fk_ae_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_empresa
    ADD CONSTRAINT fk_ae_2 FOREIGN KEY (id_empresa) REFERENCES tb_empresa(id);


--
-- TOC entry 2147 (class 2606 OID 42096)
-- Name: fk_af_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_fatura
    ADD CONSTRAINT fk_af_1 FOREIGN KEY (id_anexo) REFERENCES tb_anexo(id);


--
-- TOC entry 2148 (class 2606 OID 42101)
-- Name: fk_af_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_fatura
    ADD CONSTRAINT fk_af_2 FOREIGN KEY (id_fatura) REFERENCES tb_fatura(id);


--
-- TOC entry 2151 (class 2606 OID 42126)
-- Name: fk_ao_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_orcamento
    ADD CONSTRAINT fk_ao_1 FOREIGN KEY (id_anexo) REFERENCES tb_anexo(id);


--
-- TOC entry 2152 (class 2606 OID 42131)
-- Name: fk_ao_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_orcamento
    ADD CONSTRAINT fk_ao_2 FOREIGN KEY (id_orcamento) REFERENCES tb_orcamento(id);


--
-- TOC entry 2149 (class 2606 OID 42111)
-- Name: fk_ap_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_projeto
    ADD CONSTRAINT fk_ap_1 FOREIGN KEY (id_anexo) REFERENCES tb_anexo(id);


--
-- TOC entry 2150 (class 2606 OID 42116)
-- Name: fk_ap_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo_projeto
    ADD CONSTRAINT fk_ap_2 FOREIGN KEY (id_projeto) REFERENCES tb_projeto(id);


--
-- TOC entry 2129 (class 2606 OID 31645)
-- Name: fk_campi_projeto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_projeto
    ADD CONSTRAINT fk_campi_projeto FOREIGN KEY (id_campi) REFERENCES tb_campi(id);


--
-- TOC entry 2125 (class 2606 OID 31606)
-- Name: fk_contato_campi; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_campi
    ADD CONSTRAINT fk_contato_campi FOREIGN KEY (id_contato) REFERENCES tb_contato(id);


--
-- TOC entry 2127 (class 2606 OID 31624)
-- Name: fk_contato_empresa; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_empresa
    ADD CONSTRAINT fk_contato_empresa FOREIGN KEY (id_contato) REFERENCES tb_contato(id);


--
-- TOC entry 2122 (class 2606 OID 31583)
-- Name: fk_contato_mantenedora; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_mantenedora
    ADD CONSTRAINT fk_contato_mantenedora FOREIGN KEY (id_contato) REFERENCES tb_contato(id);


--
-- TOC entry 2134 (class 2606 OID 31681)
-- Name: fk_empresa_orcamento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento
    ADD CONSTRAINT fk_empresa_orcamento FOREIGN KEY (id_empresa) REFERENCES tb_empresa(id);


--
-- TOC entry 2126 (class 2606 OID 31611)
-- Name: fk_endereco_campi; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_campi
    ADD CONSTRAINT fk_endereco_campi FOREIGN KEY (id_endereco) REFERENCES tb_endereco(id);


--
-- TOC entry 2128 (class 2606 OID 31629)
-- Name: fk_endereco_empresa; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_empresa
    ADD CONSTRAINT fk_endereco_empresa FOREIGN KEY (id_endereco) REFERENCES tb_endereco(id);


--
-- TOC entry 2123 (class 2606 OID 31588)
-- Name: fk_endereco_mantenedora; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_mantenedora
    ADD CONSTRAINT fk_endereco_mantenedora FOREIGN KEY (id_endereco) REFERENCES tb_endereco(id);


--
-- TOC entry 2144 (class 2606 OID 31793)
-- Name: fk_fatura_compra; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_compra
    ADD CONSTRAINT fk_fatura_compra FOREIGN KEY (id_fatura) REFERENCES tb_fatura(id);


--
-- TOC entry 2143 (class 2606 OID 31777)
-- Name: fk_forma_pagamento_fatura; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_fatura
    ADD CONSTRAINT fk_forma_pagamento_fatura FOREIGN KEY (id_forma_pagamento) REFERENCES tb_forma_pagamento(id);


--
-- TOC entry 2120 (class 2606 OID 31799)
-- Name: fk_funcao_usuario; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_usuario
    ADD CONSTRAINT fk_funcao_usuario FOREIGN KEY (id_funcao) REFERENCES tb_funcao(id);


--
-- TOC entry 2124 (class 2606 OID 31601)
-- Name: fk_mantenedora_campi; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_campi
    ADD CONSTRAINT fk_mantenedora_campi FOREIGN KEY (id_mantenedora) REFERENCES tb_mantenedora(id);


--
-- TOC entry 2141 (class 2606 OID 31767)
-- Name: fk_orcamento_fatura; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_fatura
    ADD CONSTRAINT fk_orcamento_fatura FOREIGN KEY (id_orcamento) REFERENCES tb_orcamento(id);


--
-- TOC entry 2139 (class 2606 OID 31736)
-- Name: fk_os_orcamento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento_servico
    ADD CONSTRAINT fk_os_orcamento FOREIGN KEY (id_orcamento) REFERENCES tb_orcamento(id);


--
-- TOC entry 2140 (class 2606 OID 31741)
-- Name: fk_os_servico; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento_servico
    ADD CONSTRAINT fk_os_servico FOREIGN KEY (id_servico) REFERENCES tb_servico(id);


--
-- TOC entry 2135 (class 2606 OID 31686)
-- Name: fk_projeto_orcamento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento
    ADD CONSTRAINT fk_projeto_orcamento FOREIGN KEY (id_projeto) REFERENCES tb_projeto(id);


--
-- TOC entry 2137 (class 2606 OID 31717)
-- Name: fk_ps_projeto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_projeto_servico
    ADD CONSTRAINT fk_ps_projeto FOREIGN KEY (id_projeto) REFERENCES tb_projeto(id);


--
-- TOC entry 2138 (class 2606 OID 31722)
-- Name: fk_ps_servico; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_projeto_servico
    ADD CONSTRAINT fk_ps_servico FOREIGN KEY (id_servico) REFERENCES tb_servico(id);


--
-- TOC entry 2132 (class 2606 OID 31671)
-- Name: fk_status_orcamento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento
    ADD CONSTRAINT fk_status_orcamento FOREIGN KEY (id_status) REFERENCES tb_status_orcamento(id);


--
-- TOC entry 2130 (class 2606 OID 31650)
-- Name: fk_status_projeto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_projeto
    ADD CONSTRAINT fk_status_projeto FOREIGN KEY (id_status) REFERENCES tb_status_projeto(id);


--
-- TOC entry 2121 (class 2606 OID 31538)
-- Name: fk_usuario_anexo; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_anexo
    ADD CONSTRAINT fk_usuario_anexo FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id);


--
-- TOC entry 2142 (class 2606 OID 31772)
-- Name: fk_usuario_fatura; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_fatura
    ADD CONSTRAINT fk_usuario_fatura FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id);


--
-- TOC entry 2133 (class 2606 OID 31676)
-- Name: fk_usuario_orcamento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_orcamento
    ADD CONSTRAINT fk_usuario_orcamento FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id);


--
-- TOC entry 2131 (class 2606 OID 31655)
-- Name: fk_usuario_projeto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_projeto
    ADD CONSTRAINT fk_usuario_projeto FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id);


--
-- TOC entry 2136 (class 2606 OID 31703)
-- Name: fk_usuario_servico; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE  tb_servico
    ADD CONSTRAINT fk_usuario_servico FOREIGN KEY (id_usuario) REFERENCES tb_usuario(id);



