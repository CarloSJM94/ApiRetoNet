USE Master 
GO
IF DB_ID('DBRetoNet') IS NOT NULL
	DROP DATABASE DBRetoNet;
GO
	CREATE DATABASE DBRetoNet;
GO
	USE DBRetoNet
GO
	CREATE TABLE TP_Genero (
	ID		INT,
	Genero	VARCHAR(50),
	PRIMARY KEY (ID)
	)
	---
	INSERT 
	INTO	TP_Genero
			(ID, Genero)
	VALUES	(1, 'Masculino'),
			(2, 'Femenino'),
			(3, 'No Binario')
	---
	CREATE TABLE TP_Persona (
	Identificacion	INT,
	ID_Genero		INT,
	Edad			INT,
	Nombre			VARCHAR(100),
	Direccion		VARCHAR(100),
	Telefono		VARCHAR(50),
	PRIMARY KEY		(Identificacion),
	CONSTRAINT		FK_TP_Persona_TP_Genero_ID_Genero FOREIGN KEY (ID_Genero)
    REFERENCES		TP_Genero(ID)
	)
	---
	CREATE TABLE TP_Cliente (
	Identificacion_Persona	INT,
	EstadoCliente			BIT NOT NULL DEFAULT(1),
	Contraseña				VARCHAR(100),
	PRIMARY KEY				(Identificacion_Persona),
	CONSTRAINT				FK_TP_Cliente_TP_Persona_Identificacion_Persona FOREIGN KEY (Identificacion_Persona)
    REFERENCES				TP_Persona(Identificacion),
	)
	---
	CREATE TABLE TP_TipoCuenta (
	ID			INT,	
	TipoCuenta	VARCHAR(50),
	PRIMARY KEY (ID)
	)
	---
	INSERT 
	INTO	TP_TipoCuenta
			(ID, TipoCuenta)
	VALUES	(1, 'Ahorros'),
			(2, 'Corriente'),
			(3, 'Nomina')
	---
	CREATE TABLE TP_Cuenta (
	Identificacion_Persona	INT,
	Numero_Cuenta			INT,
	ID_Tipo_Cuenta			INT,
	EstadoCuenta			BIT NOT NULL DEFAULT(1),
	Saldo					MONEY,
	PRIMARY KEY				(Identificacion_Persona, Numero_Cuenta),
	UNIQUE					(Numero_Cuenta),
	CONSTRAINT				FK_TP_Cuenta_TP_Cliente_Identificacion_Persona FOREIGN KEY (Identificacion_Persona)
    REFERENCES				TP_Cliente(Identificacion_Persona),
	CONSTRAINT				FK_TP_Cuenta_TP_TipoCuenta_ID_Tipo_Cuenta FOREIGN KEY (ID_Tipo_Cuenta)
    REFERENCES				TP_TipoCuenta(ID)
	)
	---
	CREATE TABLE TP_TipoMovimiento (
	ID				INT,	
	TipoMovimiento	VARCHAR(50),
	PRIMARY KEY (ID)
	)
	---
	INSERT 
	INTO	TP_TipoMovimiento
			(ID, TipoMovimiento)
	VALUES	(1, 'Ingreso'),
			(2, 'Egreso')
	---
	CREATE TABLE TM_Movimiento (
	ID						BIGINT NOT NULL IDENTITY,
	Numero_Cuenta			INT,
	ID_TP_TipoMovimiento	INT,
	Valor					MONEY,
	Saldo					MONEY,
	FechaMovimiento			DATETIME,
	Exitoso					BIT,
	PRIMARY KEY				(ID),
	CONSTRAINT				FK_TM_Movimiento_TP_Cuenta_Numero_Numero_Cuenta FOREIGN KEY (Numero_Cuenta)
    REFERENCES				TP_Cuenta(Numero_Cuenta),
	CONSTRAINT				FK_TM_Movimiento_TP_TipoMovimiento_ID_TP_TipoMovimiento FOREIGN KEY (ID_TP_TipoMovimiento)
    REFERENCES				TP_TipoMovimiento(ID)
	)
	---
