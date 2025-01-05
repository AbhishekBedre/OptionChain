--IT

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'IT' 'MappingName' from StockMetaData where Symbol in ( 
'LTIM',
'COFORGE',
'LTTS',
'INFY',
'MPHASIS',
'PERSISTENT',
'HCLTECH',
'TCS',
'TECHM',
'WIPRO'
)


--ENEGY

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'ENERGY' 'MappingName' from StockMetaData where Symbol in ( 
'ONGC',
'IGL',
'OIL',
'THERMAX',
'TRITURBINE',
'ADANIENSOL',
'MGL',
'GUJGASLTD',
'RELIANCE',
'NLCINDIA',
'NTPC',
'GVT&D',
'PETRONET',
'TATAPOWER',
'NHPC',
'COALINDIA',
'JPPOWER',
'POWERGRID',
'IOC',
'GAIL',
'BPCL',
'GSPL',
'HINDPETRO',
'SJVN',
'POWERINDIA',
'ADANIPOWER',
'CASTROLIND',
'ADANIGREEN',
'ATGL',
'BHEL',
'JSWENERGY',
'AEGISLOG',
'SIEMENS',
'SUZLON',
'ABB',
'SCHNEIDER',
'CESC',
'TORNTPOWER',
'CGPOWER',
'INOXWIND'
)

--BANKING

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'BANKING' 'MappingName' from StockMetaData where Symbol in ( 
'IDFCFIRSTB',
'PNB',
'AUBANK',
'INDUSINDBK',
'BANKBARODA',
'CANBK',
'KOTAKBANK',
'AXISBANK',
'FEDERALBNK',
'SBIN',
'ICICIBANK',
'HDFCBANK'
)

--AUTO 

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'AUTO' 'MappingName' from StockMetaData where Symbol in ( 'TATAMOTORS',
'MARUTI',
'EICHERMOT',	
'MOTHERSON',	
'BOSCHLTD',	
'TVSMOTOR',	
'M&M',	
'ASHOKLEY',	
'EXIDEIND',	
'BAJAJ-AUTO',	
'BHARATFORG',	
'HEROMOTOCO',	
'MRF',	
'BALKRISIND',
'APOLLOTYRE'
)

--FMCG

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'FMCG' 'MappingName' from StockMetaData where Symbol in ( 
'DABUR',
'GODREJCP',
'NESTLEIND',
'HINDUNILVR',
'COLPAL',
'BRITANNIA',
'MARICO',
'PGHH',
'TATACONSUM',
'VBL',
'RADICO',
'UBL',
'UNITDSPR',
'BALRAMCHIN',
'ITC'
)

--METAL

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'METAL' 'MappingName' from StockMetaData where Symbol in ( 
'HINDZINC',
'APLAPOLLO',
'VEDL',
'JINDALSTEL',
'TATASTEEL',
'JSWSTEEL',
'SAIL',
'HINDCOPPER',
'NMDC',
'RATNAMANI',
'JSL',
'ADANIENT',
'WELCORP',
'HINDALCO',
'NATIONALUM'
)

--PSU BANK

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'PSU BANK' 'MappingName' from StockMetaData where Symbol in ( 
'MAHABANK',
'CENTRALBK',
'UNIONBANK',
'UCOBANK',
'IOB',
'PSB',
'PNB',
'BANKINDIA',
'BANKBARODA',
'INDIANB',
'CANBK',
'SBIN'
)

--PHARMA

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'PHARMA' 'MappingName' from StockMetaData where Symbol in ( 
'GLAND',
'MANKIND',
'JBCHEPHARM',
'GLENMARK',
'IPCALAB',
'GRANULES',
'LAURUSLABS',
'NATCOPHARM',
'TORNTPHARM',
'LUPIN',
'BIOCON',
'ALKEM',
'SUNPHARMA',
'ABBOTINDIA',
'DIVISLAB',
'DRREDDY',
'ZYDUSLIFE',
'CIPLA',
'AUROPHARMA',
'AJANTPHARM'
)

--OIL & GAS

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'OIL & GAS' 'MappingName' from StockMetaData where Symbol in ( 
'ONGC',
'IGL',
'OIL',
'MGL',
'GUJGASLTD',
'RELIANCE',
'PETRONET',
'IOC',
'GAIL',
'BPCL',
'GSPL',
'HINDPETRO',
'CASTROLIND',
'ATGL',
'AEGISLOG'
)

--REALITY

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'REALITY' 'MappingName' from StockMetaData where Symbol in ( 
'LODHA',
'MAHLIFE',
'BRIGADE',
'RAYMOND',
'SOBHA',
'PRESTIGE',
'DLF',
'OBEROIRLTY',
'PHOENIXLTD',
'GODREJPROP'
)

--MEDIA

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'MEDIA' 'MappingName' from StockMetaData where Symbol in ( 
'SAREGAMA',
'TIPSMUSIC',
'NETWORK18',
'ZEEL',
'HATHWAY',
'DEN',
'NAZARA',
'DISHTV',
'SUNTV',
'PVRINOX'
)

--PRIVATE BANK

Insert into Sectors (Symbol, Industry, MappingName) select Symbol, Industry, 'PRIVATE BANK' 'MappingName' from StockMetaData where Symbol in ( 
'RBLBANK',
'IDFCFIRSTB',
'INDUSINDBK',
'KOTAKBANK',
'AXISBANK',
'FEDERALBNK',
'CUB',
'ICICIBANK',
'BANDHANBNK',
'HDFCBANK'
)

--HEALTH

Insert into Sectors (Symbol, Industry, MappingName) 
select Symbol, Industry, 'HEALTH' 'MappingName' from StockMetaData where Symbol in ( 
'MAXHEALTH',
'APOLLOHOSP',
'SYNGENE',
'LALPATHLAB',
'METROPOLIS'
)

--CONSUMER DURABLES

Insert into Sectors (Symbol, Industry, MappingName) 
select Symbol, Industry, 'CONSUMER DURABLE' 'MappingName' from StockMetaData where Symbol in ( 
'BATAINDIA',
'BLUESTARCO',
'AMBER',
'WHIRLPOOL',
'TITAN',
'DIXON',
'CENTURYPLY',
'RAJESHEXPO',
'KAJARIACER',
'HAVELLS',
'KALYANKJIL',
'VOLTAS',
'CROMPTON',
'VGUARD',
'CERA'
)




