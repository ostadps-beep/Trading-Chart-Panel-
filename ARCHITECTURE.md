# Trading Chart Panel - Architecture Documentation

## Overview

This document describes the complete architecture of the Trading Chart Panel system.

## Layered Architecture

```
┌────────────────────────────────────────────────────────┐
│          PRESENTATION LAYER (WPF)                      │
│  Views, ViewModels, Controls, Input Handlers           │
└──────────────────┬─────────────────────────────────────┘
                   │
┌──────────────────▼─────────────────────────────────────┐
│          BUSINESS LOGIC LAYER (Core)                   │
│  Services, Models, State Management                    │
└──────────────────┬─────────────────────────────────────┘
                   │
┌──────────────────▼─────────────────────────────────────┐
│          ABSTRACTION LAYER (Interfaces)                │
│  IDataSource, IIndicator, IRenderer, etc               │
└──────────────────┬─────────────────────────────────────┘
                   │
┌──────────────────▼─────────────────────────────────────┐
│          DATA & INTEGRATION LAYER                      │
│  Adapters: MT4, MT5, CSV, API, Backtest                │
└────────────────────────────────────────────────────────┘
```

## Key Components

### Core Services
1. **DataManager** - Load, validate, cache OHLC data
2. **ChartStateManager** - Manage zoom, pan, scale state
3. **ViewportCalculator** - Calculate visible bars and price ranges
4. **RenderingEngine** - Coordinate chart rendering
5. **IndicatorManager** - Load and compute indicators
6. **InteractionCoordinator** - Coordinate user input
7. **AxisManager** - Price and time axis logic

## Technology Stack

- **UI Framework:** WPF
- **Charting Library:** OxyPlot
- **Language:** C# (.NET 6+ or .NET Framework 4.8+)
- **Architecture Pattern:** MVVM, Dependency Injection
- **Testing:** xUnit, Moq
