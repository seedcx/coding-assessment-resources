# Seed CX C++ Coding Assessment Resources

This repository contains tools and data sets which are helpful when solving `Seed CX C++ Coding Assessment`.

In order not to give out too many hints regarding the `C++` solution, the helper tools are written in a different language. C# was chosen since it's sufficiently different, yet easy to understand and tweak.

Provided tools include:

* [`replay-server`](#Replay-Server) which acts as the originator of market data. It replays text-based data files containing quotes and trades. Use it to feed market data into your application.

* [`order-listener`](#Order-Listener) which acts as a sink for incoming orders.

## Pre-requisites

To build and run the tools, you will need to install the free and open source `.NET Core SDK 2.2`.

Simply follow the "Build Apps" instructions relevant to your OS distribution:
https://dotnet.microsoft.com/download

## Replay Server

`replay-server` is a helpful application which plays back market data messages contained in data files.

It listens for incoming TCP connections. Upon accepting a client, the `replay-server` immediately begins sending market data records one-by-one using the binary packet format defined in the coding assessment. Byte order of the wire packets can be specified as either `big` or `little` using command line argument.

### Usage

```bash
> dotnet run -p replay-server FILE.DAT PORT BYTE_ORDER
```

where

`FILE.DAT` - path to file containing market data to replay to connecting clients

`PORT` - port to listen for connections on (ex: 65500)

`BYTE_ORDER` - specifies whether the data on the wire should be encoded using `big` or `little` endian order. Valid values are `big` or `little`


### Example output

```text
> dotnet run -p replay-server ./data/BTC-USD.dat 5000 big
Accepting replay clients on: 0.0.0.0:5000
...snip...
224606779726955|Q|BTC-USD|10688|32|10688|12
224606780961553|Q|BTC-USD|10687|57|10687|33
Finished replay
client disconnected
Accepting replay clients on: 0.0.0.0:5000
```


## Order Listener

`order-listener` is a simple application which accepts incoming connections, interprets the data as order messages, and prints them to screen. Use this tool to visualize orders transmitted by your application.

Both `big` and `little` byte orders are supported. Simplify specify the mode on command line.

### Usage

```bash
> dotnet run -p order-listener PORT BYTE_ORDER
```

where:

`PORT` - port to listen for connections on (ex: `65511`)

`BYTE_ORDER` - specifies whether the data on the wire is encoded using `big` or `little` endian order. Valid values are `big` or `little`

### Example output

```text
> dotnet run -p order-listener 6000 little
Accepting order clients on: 0.0.0.0:6000
Accepted connection: 127.0.0.1:52252
<order timeStamp="1556564220920459606" symbol="BTC-USD" side="B" price="5006" size="89" />
<order timeStamp="1556564360070459587" symbol="BTC-USD" side="B" price="5001" size="11" />
client disconnected
Accepting order clients on: 0.0.0.0:6000
Accepted connection: 127.0.0.1:52264
<order timeStamp="1556564426170459524" symbol="BTC-USDT" side="B" price="4501" size="78" />
<order timeStamp="1556564436220459537" symbol="BTC-USDT" side="B" price="4501" size="83" />
<order timeStamp="1556564496770459535" symbol="BTC-USDT" side="B" price="4501" size="52" />
client disconnected
```

