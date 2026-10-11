CREATE TABLE [dbo].[Usuarios] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Email]            VARCHAR (256)  NOT NULL,
    [EmailNormalizado] VARCHAR (256)  NOT NULL,
    [PasswordHash]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC)
);

