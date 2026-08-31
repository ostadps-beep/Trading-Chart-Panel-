# Trading Chart Panel - Professional Financial Charting for WPF

A production-quality, extensible financial trading chart component for Windows Desktop WPF applications.

## Architecture Overview

**Layered Architecture:**
- **Presentation Layer (WPF):** Views, ViewModels, Controls, Input Handlers
- **Business Logic Layer (Core):** Services, Models, State Management
- **Abstraction Layer:** Interfaces (IDataSource, IIndicator, IRenderer)
- **Data & Integration Layer:** Adapters for MT4, MT5, CSV, API, Backtest

## Key Features

- ✅ Professional candlestick charting
- ✅ MT4-style interaction (zoom, pan, mouse control)
- ✅ Real-time and historical data support
- ✅ Tick-by-tick ready architecture
- ✅ Multi-level performance optimization
- ✅ Extensible indicator framework
- ✅ Pluggable data sources
- ✅ Large dataset support (100K+ candles)
- ✅ Independent, reusable module

## Getting Started

### Prerequisites
- Visual Studio 2022+
- .NET 6.0+ / .NET Framework 4.8+
- WPF runtime

### Building
```bash
dotnet build
```

### Running Tests
```bash
dotnet test
```

## Documentation

- **ARCHITECTURE.md** - Detailed component architecture
- **docs/API.md** - Public API reference
- **docs/DESIGN.md** - Design decisions
- **docs/EXTENSIBILITY.md** - Adding indicators and adapters

## License

MIT

## Author

ostadps-beep
