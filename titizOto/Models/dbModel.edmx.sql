
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/09/2014 21:28:38
-- Generated from EDMX file: C:\Users\erkanaslanel\Documents\Visual Studio 2012\Projects\titizOto\titizOto\Models\dbModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [testcomp_titiz];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tbl_address_tbl_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_address] DROP CONSTRAINT [FK_tbl_address_tbl_user];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_adminUser_tbl_adminRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_adminUser] DROP CONSTRAINT [FK_tbl_adminUser_tbl_adminRole];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_bankEft_tbl_bank]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_bankEft] DROP CONSTRAINT [FK_tbl_bankEft_tbl_bank];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_bankPos_tbl_bank]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_bankPos] DROP CONSTRAINT [FK_tbl_bankPos_tbl_bank];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[FK_tbl_bankPosOption_tbl_bankPos]', 'F') IS NOT NULL
    ALTER TABLE [testcomp_titizUser].[tbl_bankPosOption] DROP CONSTRAINT [FK_tbl_bankPosOption_tbl_bankPos];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[FK_tbl_basket_tbl_product]', 'F') IS NOT NULL
    ALTER TABLE [testcomp_titizUser].[tbl_basket] DROP CONSTRAINT [FK_tbl_basket_tbl_product];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_carModel_tbl_carBrand]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_carModel] DROP CONSTRAINT [FK_tbl_carModel_tbl_carBrand];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_carModelProduct_tbl_carModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_carModelProduct] DROP CONSTRAINT [FK_tbl_carModelProduct_tbl_carModel];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_carModelProduct_tbl_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_carModelProduct] DROP CONSTRAINT [FK_tbl_carModelProduct_tbl_product];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_gallery_tbl_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_gallery] DROP CONSTRAINT [FK_tbl_gallery_tbl_product];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_order_tbl_cargo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_order] DROP CONSTRAINT [FK_tbl_order_tbl_cargo];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_order_tbl_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_order] DROP CONSTRAINT [FK_tbl_order_tbl_user];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_orderDetail_tbl_order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_orderDetail] DROP CONSTRAINT [FK_tbl_orderDetail_tbl_order];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_page_tbl_category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_page] DROP CONSTRAINT [FK_tbl_page_tbl_category];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_product_tbl_brand]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_product] DROP CONSTRAINT [FK_tbl_product_tbl_brand];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_product_tbl_business]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_product] DROP CONSTRAINT [FK_tbl_product_tbl_business];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_productCritear_tbl_critear]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_productCritear] DROP CONSTRAINT [FK_tbl_productCritear_tbl_critear];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_productCritear_tbl_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_productCritear] DROP CONSTRAINT [FK_tbl_productCritear_tbl_product];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_stock_tbl_product]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_stock] DROP CONSTRAINT [FK_tbl_stock_tbl_product];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[tbl_address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_address];
GO
IF OBJECT_ID(N'[dbo].[tbl_adminRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_adminRole];
GO
IF OBJECT_ID(N'[dbo].[tbl_adminUser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_adminUser];
GO
IF OBJECT_ID(N'[dbo].[tbl_bank]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_bank];
GO
IF OBJECT_ID(N'[dbo].[tbl_bankEft]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_bankEft];
GO
IF OBJECT_ID(N'[dbo].[tbl_bankPos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_bankPos];
GO
IF OBJECT_ID(N'[dbo].[tbl_brand]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_brand];
GO
IF OBJECT_ID(N'[dbo].[tbl_business]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_business];
GO
IF OBJECT_ID(N'[dbo].[tbl_carBrand]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_carBrand];
GO
IF OBJECT_ID(N'[dbo].[tbl_cargo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_cargo];
GO
IF OBJECT_ID(N'[dbo].[tbl_carModel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_carModel];
GO
IF OBJECT_ID(N'[dbo].[tbl_carModelProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_carModelProduct];
GO
IF OBJECT_ID(N'[dbo].[tbl_category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_category];
GO
IF OBJECT_ID(N'[dbo].[tbl_critear]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_critear];
GO
IF OBJECT_ID(N'[dbo].[tbl_gallery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_gallery];
GO
IF OBJECT_ID(N'[dbo].[tbl_module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_module];
GO
IF OBJECT_ID(N'[dbo].[tbl_order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_order];
GO
IF OBJECT_ID(N'[dbo].[tbl_orderDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_orderDetail];
GO
IF OBJECT_ID(N'[dbo].[tbl_page]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_page];
GO
IF OBJECT_ID(N'[dbo].[tbl_payInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_payInfo];
GO
IF OBJECT_ID(N'[dbo].[tbl_product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_product];
GO
IF OBJECT_ID(N'[dbo].[tbl_productCritear]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_productCritear];
GO
IF OBJECT_ID(N'[dbo].[tbl_settings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_settings];
GO
IF OBJECT_ID(N'[dbo].[tbl_slider]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_slider];
GO
IF OBJECT_ID(N'[dbo].[tbl_stock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_stock];
GO
IF OBJECT_ID(N'[dbo].[tbl_user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_user];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_activation]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_activation];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_bankPosOption]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_bankPosOption];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_basket]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_basket];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_discount]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_discount];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_email]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_email];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_error]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_error];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_newsletterUser]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_newsletterUser];
GO
IF OBJECT_ID(N'[testcomp_titizUser].[tbl_openContent]', 'U') IS NOT NULL
    DROP TABLE [testcomp_titizUser].[tbl_openContent];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tbl_address'
CREATE TABLE [dbo].[tbl_address] (
    [addressId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [isPersonal] bit  NOT NULL,
    [userId] int  NOT NULL,
    [isDeleted] bit  NOT NULL
);
GO

-- Creating table 'tbl_adminRole'
CREATE TABLE [dbo].[tbl_adminRole] (
    [roleId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_adminUser'
CREATE TABLE [dbo].[tbl_adminUser] (
    [userId] int IDENTITY(1,1) NOT NULL,
    [email] nvarchar(500)  NOT NULL,
    [password] nvarchar(500)  NOT NULL,
    [enterDate] datetime  NOT NULL,
    [adminRoleId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [surname] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_bank'
CREATE TABLE [dbo].[tbl_bank] (
    [bankId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [color1] nvarchar(500)  NULL,
    [color2] nvarchar(500)  NULL,
    [color3] nvarchar(500)  NULL,
    [logo] nvarchar(500)  NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [paymentLogo] nvarchar(500)  NULL
);
GO

-- Creating table 'tbl_bankEft'
CREATE TABLE [dbo].[tbl_bankEft] (
    [bankEftId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [bankId] int  NOT NULL,
    [branchCode] nvarchar(50)  NOT NULL,
    [accountNo] nvarchar(50)  NOT NULL,
    [iban] nvarchar(50)  NOT NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL
);
GO

-- Creating table 'tbl_bankPos'
CREATE TABLE [dbo].[tbl_bankPos] (
    [bankPosId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [bankId] int  NOT NULL,
    [statu] bit  NOT NULL
);
GO

-- Creating table 'tbl_business'
CREATE TABLE [dbo].[tbl_business] (
    [businessId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_carBrand'
CREATE TABLE [dbo].[tbl_carBrand] (
    [carBrandId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [url] nvarchar(500)  NOT NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [isManuelUrl] bit  NOT NULL,
    [photo] nvarchar(500)  NOT NULL,
    [photoCoordinate] nvarchar(500)  NOT NULL,
    [langId] int  NOT NULL,
    [isMainPageShown] bit  NOT NULL
);
GO

-- Creating table 'tbl_cargo'
CREATE TABLE [dbo].[tbl_cargo] (
    [cargoId] int IDENTITY(1,1) NOT NULL,
    [price] decimal(10,2)  NOT NULL,
    [freeCargoPrice] decimal(10,2)  NOT NULL,
    [photo] nvarchar(500)  NOT NULL,
    [photoCoordinate] nvarchar(500)  NOT NULL,
    [name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_carModel'
CREATE TABLE [dbo].[tbl_carModel] (
    [carModelId] int IDENTITY(1,1) NOT NULL,
    [carBrandId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [url] nvarchar(500)  NOT NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [isManuelUrl] bit  NOT NULL,
    [langId] int  NOT NULL
);
GO

-- Creating table 'tbl_carModelProduct'
CREATE TABLE [dbo].[tbl_carModelProduct] (
    [carModelProductId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [carModelId] int  NOT NULL,
    [sequence] int  NOT NULL
);
GO

-- Creating table 'tbl_category'
CREATE TABLE [dbo].[tbl_category] (
    [categoryId] int IDENTITY(1,1) NOT NULL,
    [langId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [parentId] int  NOT NULL,
    [sequence] int  NOT NULL,
    [statu] bit  NOT NULL,
    [isMainMenuShown] bit  NOT NULL,
    [isFooterMenuShown] bit  NOT NULL
);
GO

-- Creating table 'tbl_order'
CREATE TABLE [dbo].[tbl_order] (
    [orderId] int IDENTITY(1,1) NOT NULL,
    [orderNo] nvarchar(50)  NOT NULL,
    [cargoId] int  NOT NULL,
    [userId] int  NOT NULL,
    [addressId] int  NOT NULL,
    [paymentTypeId] int  NOT NULL,
    [cargoPrice] decimal(10,2)  NOT NULL,
    [orderStatu] int  NOT NULL,
    [orderMailStatu] int  NOT NULL,
    [totalPrice] decimal(10,2)  NOT NULL
);
GO

-- Creating table 'tbl_orderDetail'
CREATE TABLE [dbo].[tbl_orderDetail] (
    [orderDetailId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [quantity] int  NOT NULL,
    [productPrice] decimal(10,2)  NOT NULL,
    [orderId] int  NOT NULL,
    [critearId] int  NOT NULL
);
GO

-- Creating table 'tbl_page'
CREATE TABLE [dbo].[tbl_page] (
    [pageId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [url] nvarchar(500)  NOT NULL,
    [langId] int  NOT NULL,
    [title] nvarchar(500)  NULL,
    [metaKeyword] nvarchar(500)  NULL,
    [metaDescription] nvarchar(500)  NULL,
    [categoryId] int  NOT NULL,
    [sequence] int  NOT NULL,
    [statu] bit  NOT NULL,
    [pageTypeId] int  NOT NULL,
    [redirectPageUrl] nvarchar(500)  NULL,
    [isManuelUrl] bit  NOT NULL,
    [detail] nvarchar(max)  NULL,
    [isHelperUrl] bit  NOT NULL
);
GO

-- Creating table 'tbl_payInfo'
CREATE TABLE [dbo].[tbl_payInfo] (
    [payInfoId] int IDENTITY(1,1) NOT NULL,
    [userId] int  NOT NULL,
    [payTime] datetime  NOT NULL,
    [processId] int  NOT NULL
);
GO

-- Creating table 'tbl_product'
CREATE TABLE [dbo].[tbl_product] (
    [productId] int IDENTITY(1,1) NOT NULL,
    [isDeleted] bit  NOT NULL,
    [brandId] int  NOT NULL,
    [businessId] int  NOT NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [price] decimal(10,2)  NOT NULL,
    [isTaxInclude] bit  NOT NULL,
    [taxPercent] int  NOT NULL,
    [discountPrice] decimal(10,2)  NOT NULL,
    [isDiscountPriceActive] bit  NOT NULL,
    [shortDescription] nvarchar(500)  NULL,
    [detail] nvarchar(max)  NULL,
    [metaKeyword] nvarchar(500)  NULL,
    [metaDescription] nvarchar(500)  NULL,
    [title] nvarchar(500)  NULL,
    [name] nvarchar(500)  NOT NULL,
    [url] nvarchar(500)  NOT NULL,
    [isManuelUrl] bit  NOT NULL,
    [langId] int  NOT NULL,
    [isShowCase] bit  NOT NULL
);
GO

-- Creating table 'tbl_settings'
CREATE TABLE [dbo].[tbl_settings] (
    [settingsId] int IDENTITY(1,1) NOT NULL,
    [langId] int  NOT NULL,
    [mailUserName] nvarchar(500)  NULL,
    [mailPassword] nvarchar(500)  NULL,
    [mailPort] nvarchar(500)  NULL,
    [mailSentName] nvarchar(500)  NULL,
    [mailSentAddress] nvarchar(500)  NULL,
    [mailReceiverAddress] nvarchar(500)  NULL,
    [mailSmtpServer] nvarchar(500)  NULL,
    [metaDescription] nvarchar(4000)  NULL,
    [metaDescriptionAdditional] nvarchar(4000)  NULL,
    [mainPageTitle] nvarchar(500)  NULL,
    [allPageTitle] nvarchar(500)  NULL,
    [metaKeyword] nvarchar(4000)  NULL,
    [registerIsActivationExist] bit  NOT NULL,
    [registerIsThankMessageSend] bit  NOT NULL,
    [mailIsEnableSSL] bit  NULL
);
GO

-- Creating table 'tbl_slider'
CREATE TABLE [dbo].[tbl_slider] (
    [sliderId] int IDENTITY(1,1) NOT NULL,
    [langId] int  NOT NULL,
    [title] nvarchar(500)  NULL,
    [subTitle] nvarchar(500)  NULL,
    [isUrlActive] bit  NOT NULL,
    [urlText] nvarchar(500)  NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [photo] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_user'
CREATE TABLE [dbo].[tbl_user] (
    [userId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [surname] nvarchar(500)  NOT NULL,
    [email] nvarchar(500)  NOT NULL,
    [password] nvarchar(500)  NULL,
    [birthday] datetime  NULL,
    [gender] int  NULL,
    [userTypeId] int  NOT NULL,
    [registerStatuId] int  NOT NULL,
    [guid] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_module'
CREATE TABLE [dbo].[tbl_module] (
    [moduleId] int  NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [htmlContent] nvarchar(500)  NOT NULL,
    [tag] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_critear'
CREATE TABLE [dbo].[tbl_critear] (
    [critearId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL,
    [parentId] int  NOT NULL,
    [sequence] int  NOT NULL,
    [statu] bit  NOT NULL
);
GO

-- Creating table 'tbl_productCritear'
CREATE TABLE [dbo].[tbl_productCritear] (
    [productCritearId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [critearId] int  NOT NULL,
    [sequence] int  NOT NULL
);
GO

-- Creating table 'tbl_newsletterUser'
CREATE TABLE [dbo].[tbl_newsletterUser] (
    [newsletterUserId] int IDENTITY(1,1) NOT NULL,
    [email] nvarchar(500)  NOT NULL,
    [ipNo] nvarchar(500)  NOT NULL,
    [createTime] datetime  NOT NULL
);
GO

-- Creating table 'tbl_brand'
CREATE TABLE [dbo].[tbl_brand] (
    [brandId] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_basket'
CREATE TABLE [dbo].[tbl_basket] (
    [basketId] int  NOT NULL,
    [productId] int  NOT NULL,
    [quantity] int  NOT NULL,
    [guestCode] nvarchar(50)  NULL,
    [createTime] datetime  NOT NULL,
    [userId] int  NULL,
    [optionList] nvarchar(50)  NULL,
    [discountCode] nvarchar(50)  NULL
);
GO

-- Creating table 'tbl_gallery'
CREATE TABLE [dbo].[tbl_gallery] (
    [galleryId] int IDENTITY(1,1) NOT NULL,
    [productId] int  NOT NULL,
    [photo] nvarchar(500)  NOT NULL,
    [photoCoordinate] nvarchar(500)  NOT NULL,
    [sequence] int  NOT NULL,
    [statu] bit  NOT NULL,
    [guid] nvarchar(500)  NOT NULL,
    [optionList] nvarchar(50)  NULL
);
GO

-- Creating table 'tbl_stock'
CREATE TABLE [dbo].[tbl_stock] (
    [stockId] int IDENTITY(1,1) NOT NULL,
    [stockCount] int  NOT NULL,
    [productId] int  NOT NULL,
    [minCount] int  NOT NULL,
    [sequence] int  NOT NULL,
    [optionList] nvarchar(50)  NULL
);
GO

-- Creating table 'tbl_openContent'
CREATE TABLE [dbo].[tbl_openContent] (
    [openContentId] int IDENTITY(1,1) NOT NULL,
    [categoryId] int  NOT NULL,
    [langId] int  NOT NULL,
    [statu] bit  NOT NULL,
    [sequence] int  NOT NULL,
    [title] nvarchar(4000)  NOT NULL,
    [detail] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tbl_discount'
CREATE TABLE [dbo].[tbl_discount] (
    [discountId] int IDENTITY(1,1) NOT NULL,
    [code] nchar(50)  NOT NULL,
    [description] nvarchar(500)  NULL,
    [typeId] int  NOT NULL,
    [amountPercent] decimal(18,2)  NOT NULL,
    [minBasketAmount] decimal(18,2)  NOT NULL,
    [minBasketCount] int  NOT NULL,
    [startDate] datetime  NOT NULL,
    [endDate] datetime  NOT NULL,
    [userId] int  NOT NULL,
    [productList] nvarchar(4000)  NULL,
    [isOtherCombine] bit  NOT NULL,
    [statu] bit  NOT NULL,
    [repTime] int  NOT NULL,
    [isManuelGenerated] bit  NOT NULL,
    [exculudeProductList] nvarchar(4000)  NULL
);
GO

-- Creating table 'tbl_bankPosOption'
CREATE TABLE [dbo].[tbl_bankPosOption] (
    [bankPosOptionId] int IDENTITY(1,1) NOT NULL,
    [paymentCount] int  NOT NULL,
    [minBasketAmount] decimal(16,2)  NOT NULL,
    [additionalAmount] decimal(10,2)  NOT NULL,
    [statu] bit  NOT NULL,
    [bankPosId] int  NOT NULL
);
GO

-- Creating table 'tbl_activation'
CREATE TABLE [dbo].[tbl_activation] (
    [activationId] int IDENTITY(1,1) NOT NULL,
    [code] nvarchar(500)  NOT NULL,
    [userId] int  NOT NULL,
    [datetime] datetime  NOT NULL
);
GO

-- Creating table 'tbl_email'
CREATE TABLE [dbo].[tbl_email] (
    [emailId] int  NOT NULL,
    [emailTypeId] int  NOT NULL,
    [langId] int  NOT NULL,
    [description] nvarchar(4000)  NULL,
    [detail] nvarchar(max)  NOT NULL,
    [title] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'tbl_error'
CREATE TABLE [dbo].[tbl_error] (
    [errorId] int IDENTITY(1,1) NOT NULL,
    [errorText] nvarchar(4000)  NOT NULL,
    [saveDate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [addressId] in table 'tbl_address'
ALTER TABLE [dbo].[tbl_address]
ADD CONSTRAINT [PK_tbl_address]
    PRIMARY KEY CLUSTERED ([addressId] ASC);
GO

-- Creating primary key on [roleId] in table 'tbl_adminRole'
ALTER TABLE [dbo].[tbl_adminRole]
ADD CONSTRAINT [PK_tbl_adminRole]
    PRIMARY KEY CLUSTERED ([roleId] ASC);
GO

-- Creating primary key on [userId] in table 'tbl_adminUser'
ALTER TABLE [dbo].[tbl_adminUser]
ADD CONSTRAINT [PK_tbl_adminUser]
    PRIMARY KEY CLUSTERED ([userId] ASC);
GO

-- Creating primary key on [bankId] in table 'tbl_bank'
ALTER TABLE [dbo].[tbl_bank]
ADD CONSTRAINT [PK_tbl_bank]
    PRIMARY KEY CLUSTERED ([bankId] ASC);
GO

-- Creating primary key on [bankEftId] in table 'tbl_bankEft'
ALTER TABLE [dbo].[tbl_bankEft]
ADD CONSTRAINT [PK_tbl_bankEft]
    PRIMARY KEY CLUSTERED ([bankEftId] ASC);
GO

-- Creating primary key on [bankPosId] in table 'tbl_bankPos'
ALTER TABLE [dbo].[tbl_bankPos]
ADD CONSTRAINT [PK_tbl_bankPos]
    PRIMARY KEY CLUSTERED ([bankPosId] ASC);
GO

-- Creating primary key on [businessId] in table 'tbl_business'
ALTER TABLE [dbo].[tbl_business]
ADD CONSTRAINT [PK_tbl_business]
    PRIMARY KEY CLUSTERED ([businessId] ASC);
GO

-- Creating primary key on [carBrandId] in table 'tbl_carBrand'
ALTER TABLE [dbo].[tbl_carBrand]
ADD CONSTRAINT [PK_tbl_carBrand]
    PRIMARY KEY CLUSTERED ([carBrandId] ASC);
GO

-- Creating primary key on [cargoId] in table 'tbl_cargo'
ALTER TABLE [dbo].[tbl_cargo]
ADD CONSTRAINT [PK_tbl_cargo]
    PRIMARY KEY CLUSTERED ([cargoId] ASC);
GO

-- Creating primary key on [carModelId] in table 'tbl_carModel'
ALTER TABLE [dbo].[tbl_carModel]
ADD CONSTRAINT [PK_tbl_carModel]
    PRIMARY KEY CLUSTERED ([carModelId] ASC);
GO

-- Creating primary key on [carModelProductId] in table 'tbl_carModelProduct'
ALTER TABLE [dbo].[tbl_carModelProduct]
ADD CONSTRAINT [PK_tbl_carModelProduct]
    PRIMARY KEY CLUSTERED ([carModelProductId] ASC);
GO

-- Creating primary key on [categoryId] in table 'tbl_category'
ALTER TABLE [dbo].[tbl_category]
ADD CONSTRAINT [PK_tbl_category]
    PRIMARY KEY CLUSTERED ([categoryId] ASC);
GO

-- Creating primary key on [orderId] in table 'tbl_order'
ALTER TABLE [dbo].[tbl_order]
ADD CONSTRAINT [PK_tbl_order]
    PRIMARY KEY CLUSTERED ([orderId] ASC);
GO

-- Creating primary key on [orderDetailId] in table 'tbl_orderDetail'
ALTER TABLE [dbo].[tbl_orderDetail]
ADD CONSTRAINT [PK_tbl_orderDetail]
    PRIMARY KEY CLUSTERED ([orderDetailId] ASC);
GO

-- Creating primary key on [pageId] in table 'tbl_page'
ALTER TABLE [dbo].[tbl_page]
ADD CONSTRAINT [PK_tbl_page]
    PRIMARY KEY CLUSTERED ([pageId] ASC);
GO

-- Creating primary key on [payInfoId] in table 'tbl_payInfo'
ALTER TABLE [dbo].[tbl_payInfo]
ADD CONSTRAINT [PK_tbl_payInfo]
    PRIMARY KEY CLUSTERED ([payInfoId] ASC);
GO

-- Creating primary key on [productId] in table 'tbl_product'
ALTER TABLE [dbo].[tbl_product]
ADD CONSTRAINT [PK_tbl_product]
    PRIMARY KEY CLUSTERED ([productId] ASC);
GO

-- Creating primary key on [settingsId] in table 'tbl_settings'
ALTER TABLE [dbo].[tbl_settings]
ADD CONSTRAINT [PK_tbl_settings]
    PRIMARY KEY CLUSTERED ([settingsId] ASC);
GO

-- Creating primary key on [sliderId] in table 'tbl_slider'
ALTER TABLE [dbo].[tbl_slider]
ADD CONSTRAINT [PK_tbl_slider]
    PRIMARY KEY CLUSTERED ([sliderId] ASC);
GO

-- Creating primary key on [userId] in table 'tbl_user'
ALTER TABLE [dbo].[tbl_user]
ADD CONSTRAINT [PK_tbl_user]
    PRIMARY KEY CLUSTERED ([userId] ASC);
GO

-- Creating primary key on [moduleId] in table 'tbl_module'
ALTER TABLE [dbo].[tbl_module]
ADD CONSTRAINT [PK_tbl_module]
    PRIMARY KEY CLUSTERED ([moduleId] ASC);
GO

-- Creating primary key on [critearId] in table 'tbl_critear'
ALTER TABLE [dbo].[tbl_critear]
ADD CONSTRAINT [PK_tbl_critear]
    PRIMARY KEY CLUSTERED ([critearId] ASC);
GO

-- Creating primary key on [productCritearId] in table 'tbl_productCritear'
ALTER TABLE [dbo].[tbl_productCritear]
ADD CONSTRAINT [PK_tbl_productCritear]
    PRIMARY KEY CLUSTERED ([productCritearId] ASC);
GO

-- Creating primary key on [newsletterUserId] in table 'tbl_newsletterUser'
ALTER TABLE [dbo].[tbl_newsletterUser]
ADD CONSTRAINT [PK_tbl_newsletterUser]
    PRIMARY KEY CLUSTERED ([newsletterUserId] ASC);
GO

-- Creating primary key on [brandId] in table 'tbl_brand'
ALTER TABLE [dbo].[tbl_brand]
ADD CONSTRAINT [PK_tbl_brand]
    PRIMARY KEY CLUSTERED ([brandId] ASC);
GO

-- Creating primary key on [basketId] in table 'tbl_basket'
ALTER TABLE [dbo].[tbl_basket]
ADD CONSTRAINT [PK_tbl_basket]
    PRIMARY KEY CLUSTERED ([basketId] ASC);
GO

-- Creating primary key on [galleryId] in table 'tbl_gallery'
ALTER TABLE [dbo].[tbl_gallery]
ADD CONSTRAINT [PK_tbl_gallery]
    PRIMARY KEY CLUSTERED ([galleryId] ASC);
GO

-- Creating primary key on [stockId] in table 'tbl_stock'
ALTER TABLE [dbo].[tbl_stock]
ADD CONSTRAINT [PK_tbl_stock]
    PRIMARY KEY CLUSTERED ([stockId] ASC);
GO

-- Creating primary key on [openContentId] in table 'tbl_openContent'
ALTER TABLE [dbo].[tbl_openContent]
ADD CONSTRAINT [PK_tbl_openContent]
    PRIMARY KEY CLUSTERED ([openContentId] ASC);
GO

-- Creating primary key on [discountId] in table 'tbl_discount'
ALTER TABLE [dbo].[tbl_discount]
ADD CONSTRAINT [PK_tbl_discount]
    PRIMARY KEY CLUSTERED ([discountId] ASC);
GO

-- Creating primary key on [bankPosOptionId] in table 'tbl_bankPosOption'
ALTER TABLE [dbo].[tbl_bankPosOption]
ADD CONSTRAINT [PK_tbl_bankPosOption]
    PRIMARY KEY CLUSTERED ([bankPosOptionId] ASC);
GO

-- Creating primary key on [activationId] in table 'tbl_activation'
ALTER TABLE [dbo].[tbl_activation]
ADD CONSTRAINT [PK_tbl_activation]
    PRIMARY KEY CLUSTERED ([activationId] ASC);
GO

-- Creating primary key on [emailId] in table 'tbl_email'
ALTER TABLE [dbo].[tbl_email]
ADD CONSTRAINT [PK_tbl_email]
    PRIMARY KEY CLUSTERED ([emailId] ASC);
GO

-- Creating primary key on [errorId] in table 'tbl_error'
ALTER TABLE [dbo].[tbl_error]
ADD CONSTRAINT [PK_tbl_error]
    PRIMARY KEY CLUSTERED ([errorId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [userId] in table 'tbl_address'
ALTER TABLE [dbo].[tbl_address]
ADD CONSTRAINT [FK_tbl_address_tbl_user]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[tbl_user]
        ([userId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_address_tbl_user'
CREATE INDEX [IX_FK_tbl_address_tbl_user]
ON [dbo].[tbl_address]
    ([userId]);
GO

-- Creating foreign key on [adminRoleId] in table 'tbl_adminUser'
ALTER TABLE [dbo].[tbl_adminUser]
ADD CONSTRAINT [FK_tbl_adminUser_tbl_adminRole]
    FOREIGN KEY ([adminRoleId])
    REFERENCES [dbo].[tbl_adminRole]
        ([roleId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_adminUser_tbl_adminRole'
CREATE INDEX [IX_FK_tbl_adminUser_tbl_adminRole]
ON [dbo].[tbl_adminUser]
    ([adminRoleId]);
GO

-- Creating foreign key on [bankId] in table 'tbl_bankEft'
ALTER TABLE [dbo].[tbl_bankEft]
ADD CONSTRAINT [FK_tbl_bankEft_tbl_bank]
    FOREIGN KEY ([bankId])
    REFERENCES [dbo].[tbl_bank]
        ([bankId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_bankEft_tbl_bank'
CREATE INDEX [IX_FK_tbl_bankEft_tbl_bank]
ON [dbo].[tbl_bankEft]
    ([bankId]);
GO

-- Creating foreign key on [bankId] in table 'tbl_bankPos'
ALTER TABLE [dbo].[tbl_bankPos]
ADD CONSTRAINT [FK_tbl_bankPos_tbl_bank]
    FOREIGN KEY ([bankId])
    REFERENCES [dbo].[tbl_bank]
        ([bankId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_bankPos_tbl_bank'
CREATE INDEX [IX_FK_tbl_bankPos_tbl_bank]
ON [dbo].[tbl_bankPos]
    ([bankId]);
GO

-- Creating foreign key on [businessId] in table 'tbl_product'
ALTER TABLE [dbo].[tbl_product]
ADD CONSTRAINT [FK_tbl_product_tbl_business]
    FOREIGN KEY ([businessId])
    REFERENCES [dbo].[tbl_business]
        ([businessId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_product_tbl_business'
CREATE INDEX [IX_FK_tbl_product_tbl_business]
ON [dbo].[tbl_product]
    ([businessId]);
GO

-- Creating foreign key on [carBrandId] in table 'tbl_carModel'
ALTER TABLE [dbo].[tbl_carModel]
ADD CONSTRAINT [FK_tbl_carModel_tbl_carBrand]
    FOREIGN KEY ([carBrandId])
    REFERENCES [dbo].[tbl_carBrand]
        ([carBrandId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_carModel_tbl_carBrand'
CREATE INDEX [IX_FK_tbl_carModel_tbl_carBrand]
ON [dbo].[tbl_carModel]
    ([carBrandId]);
GO

-- Creating foreign key on [cargoId] in table 'tbl_order'
ALTER TABLE [dbo].[tbl_order]
ADD CONSTRAINT [FK_tbl_order_tbl_cargo]
    FOREIGN KEY ([cargoId])
    REFERENCES [dbo].[tbl_cargo]
        ([cargoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_order_tbl_cargo'
CREATE INDEX [IX_FK_tbl_order_tbl_cargo]
ON [dbo].[tbl_order]
    ([cargoId]);
GO

-- Creating foreign key on [carModelId] in table 'tbl_carModelProduct'
ALTER TABLE [dbo].[tbl_carModelProduct]
ADD CONSTRAINT [FK_tbl_carModelProduct_tbl_carModel]
    FOREIGN KEY ([carModelId])
    REFERENCES [dbo].[tbl_carModel]
        ([carModelId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_carModelProduct_tbl_carModel'
CREATE INDEX [IX_FK_tbl_carModelProduct_tbl_carModel]
ON [dbo].[tbl_carModelProduct]
    ([carModelId]);
GO

-- Creating foreign key on [productId] in table 'tbl_carModelProduct'
ALTER TABLE [dbo].[tbl_carModelProduct]
ADD CONSTRAINT [FK_tbl_carModelProduct_tbl_product]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[tbl_product]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_carModelProduct_tbl_product'
CREATE INDEX [IX_FK_tbl_carModelProduct_tbl_product]
ON [dbo].[tbl_carModelProduct]
    ([productId]);
GO

-- Creating foreign key on [categoryId] in table 'tbl_page'
ALTER TABLE [dbo].[tbl_page]
ADD CONSTRAINT [FK_tbl_page_tbl_category]
    FOREIGN KEY ([categoryId])
    REFERENCES [dbo].[tbl_category]
        ([categoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_page_tbl_category'
CREATE INDEX [IX_FK_tbl_page_tbl_category]
ON [dbo].[tbl_page]
    ([categoryId]);
GO

-- Creating foreign key on [userId] in table 'tbl_order'
ALTER TABLE [dbo].[tbl_order]
ADD CONSTRAINT [FK_tbl_order_tbl_user]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[tbl_user]
        ([userId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_order_tbl_user'
CREATE INDEX [IX_FK_tbl_order_tbl_user]
ON [dbo].[tbl_order]
    ([userId]);
GO

-- Creating foreign key on [orderId] in table 'tbl_orderDetail'
ALTER TABLE [dbo].[tbl_orderDetail]
ADD CONSTRAINT [FK_tbl_orderDetail_tbl_order]
    FOREIGN KEY ([orderId])
    REFERENCES [dbo].[tbl_order]
        ([orderId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_orderDetail_tbl_order'
CREATE INDEX [IX_FK_tbl_orderDetail_tbl_order]
ON [dbo].[tbl_orderDetail]
    ([orderId]);
GO

-- Creating foreign key on [critearId] in table 'tbl_productCritear'
ALTER TABLE [dbo].[tbl_productCritear]
ADD CONSTRAINT [FK_tbl_productCritear_tbl_critear]
    FOREIGN KEY ([critearId])
    REFERENCES [dbo].[tbl_critear]
        ([critearId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_productCritear_tbl_critear'
CREATE INDEX [IX_FK_tbl_productCritear_tbl_critear]
ON [dbo].[tbl_productCritear]
    ([critearId]);
GO

-- Creating foreign key on [productId] in table 'tbl_productCritear'
ALTER TABLE [dbo].[tbl_productCritear]
ADD CONSTRAINT [FK_tbl_productCritear_tbl_product]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[tbl_product]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_productCritear_tbl_product'
CREATE INDEX [IX_FK_tbl_productCritear_tbl_product]
ON [dbo].[tbl_productCritear]
    ([productId]);
GO

-- Creating foreign key on [brandId] in table 'tbl_product'
ALTER TABLE [dbo].[tbl_product]
ADD CONSTRAINT [FK_tbl_product_tbl_brand]
    FOREIGN KEY ([brandId])
    REFERENCES [dbo].[tbl_brand]
        ([brandId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_product_tbl_brand'
CREATE INDEX [IX_FK_tbl_product_tbl_brand]
ON [dbo].[tbl_product]
    ([brandId]);
GO

-- Creating foreign key on [productId] in table 'tbl_basket'
ALTER TABLE [dbo].[tbl_basket]
ADD CONSTRAINT [FK_tbl_basket_tbl_product]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[tbl_product]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_basket_tbl_product'
CREATE INDEX [IX_FK_tbl_basket_tbl_product]
ON [dbo].[tbl_basket]
    ([productId]);
GO

-- Creating foreign key on [productId] in table 'tbl_gallery'
ALTER TABLE [dbo].[tbl_gallery]
ADD CONSTRAINT [FK_tbl_gallery_tbl_product]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[tbl_product]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_gallery_tbl_product'
CREATE INDEX [IX_FK_tbl_gallery_tbl_product]
ON [dbo].[tbl_gallery]
    ([productId]);
GO

-- Creating foreign key on [productId] in table 'tbl_stock'
ALTER TABLE [dbo].[tbl_stock]
ADD CONSTRAINT [FK_tbl_stock_tbl_product]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[tbl_product]
        ([productId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_stock_tbl_product'
CREATE INDEX [IX_FK_tbl_stock_tbl_product]
ON [dbo].[tbl_stock]
    ([productId]);
GO

-- Creating foreign key on [bankPosId] in table 'tbl_bankPosOption'
ALTER TABLE [dbo].[tbl_bankPosOption]
ADD CONSTRAINT [FK_tbl_bankPosOption_tbl_bankPos]
    FOREIGN KEY ([bankPosId])
    REFERENCES [dbo].[tbl_bankPos]
        ([bankPosId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_bankPosOption_tbl_bankPos'
CREATE INDEX [IX_FK_tbl_bankPosOption_tbl_bankPos]
ON [dbo].[tbl_bankPosOption]
    ([bankPosId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------