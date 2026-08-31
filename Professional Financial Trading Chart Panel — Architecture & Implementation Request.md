# Professional Financial Trading Chart Panel — Architecture & Implementation Request

Design and implement a **professional, extensible financial trading chart panel** for a Windows desktop Forex analysis application.

## Primary Objective

Build a production-quality financial chart component suitable for a serious Forex/trading analysis system.

The Chart Panel itself is a **standalone, independent module**. It must not be designed around Forex Analyzer or any other specific host application. Any future host system must integrate with the panel through a clearly defined, stable interface/contract and adapt itself to the panel's contract rather than requiring the panel to be redesigned for that host.

The chart must NOT be designed as a simple demonstration, toy project, or one-off visualization.

The panel must remain usable and testable independently of its eventual host application and must not contain business logic belonging to a trading system, strategy, analysis engine, broker integration, or specific data provider.

It must be:

- Professional
- Financial/trading oriented
- Extensible
- Maintainable
- Performant
- Modular
- Suitable for large amounts of market data
- Designed for future expansion
- Independent from any specific trading strategy
- Capable of supporting additional financial features without requiring a redesign of the core architecture

## Important Design Principle

Do **not** assume that the requirements for mouse interaction, zooming, panning, scaling, chart navigation, axes, rendering, or other chart behavior should be dictated by the requester.

Determine the appropriate behavior from:

1. Established financial charting conventions
2. Professional trading platforms
3. Industry-standard UX patterns for financial charts
4. The capabilities and recommended architecture of the selected technology
5. Performance and scalability requirements
6. Sound software-engineering principles

Do not blindly implement arbitrary interaction rules merely because they are common in ordinary UI applications.

Where multiple valid approaches exist, evaluate them and select the architecture that is most appropriate for a professional financial trading system.

## Technology Selection

Before implementation:

- Evaluate suitable charting technologies/libraries.
- Evaluate whether an existing professional chart component should be used instead of implementing chart functionality from scratch.
- Prefer a mature and extensible solution when appropriate.
- Do not reinvent functionality that a reliable charting framework already provides unless there is a strong technical reason.
- Explain the selected technology and architecture before implementation.

The solution must be compatible with a modern Windows desktop application and must allow future development without locking the project into an unnecessarily restrictive architecture.

The technology choice must be appropriate for an independently reusable Windows financial-chart module. Compatibility with a future host application is required, but the panel must not become dependent on that host application's internal architecture.

## Financial Chart Requirements

The system should be capable of supporting standard financial chart functionality, including as appropriate:

- OHLC data
- Candlestick charts
- Line charts
- Bar charts
- Volume
- Time-based market data
- Price scales
- Time scales
- Multiple timeframes
- Historical data
- Real-time data
- Chart navigation
- Standard financial chart interaction
- Crosshair
- Data inspection
- Dynamic updates
- Large historical datasets

Do not artificially limit the design to only the currently required features.

The architecture must allow additional chart types, indicators, drawing tools, overlays and analytical components to be added later.

## Interaction Design

The interaction model must be designed according to professional financial-chart conventions rather than arbitrary assumptions.

Evaluate and implement appropriate behavior for:

- Mouse interaction
- Keyboard interaction
- Chart navigation
- Zooming
- Panning
- Price-scale interaction
- Time-axis interaction
- Crosshair interaction
- Context menus
- Selection
- Data inspection
- Automatic scrolling
- Historical navigation

The interaction system must remain modular so that future behavior can be added or changed without rewriting the chart rendering system.

## Architecture

Separate the system into logical components.

At minimum, consider separation between:

- Market data model
- Data management
- Chart rendering
- Chart state
- Axis management
- Interaction/input handling
- Crosshair/data inspection
- Indicators
- Drawing/annotation tools
- Configuration
- External data sources

Do not create a single large controller containing all chart behavior.

The architecture should follow appropriate principles such as:

- Separation of concerns
- Low coupling
- High cohesion
- Reusability
- Testability
- Dependency inversion where appropriate
- Clear interfaces between components

## Performance

The chart must be designed for real financial datasets.

Do not assume that only a few hundred candles will ever be displayed.

Consider:

- Thousands to millions of historical records
- Efficient rendering
- Efficient data updates
- Memory usage
- Incremental updates
- Real-time updates
- Avoiding unnecessary redraws
- Efficient viewport calculations
- Efficient interaction

The architecture should allow performance optimizations later without requiring a complete rewrite.

## Data Architecture

The chart must not be tightly coupled to one specific data source.

The design should allow market data to originate from different sources in the future, such as:

- MT4
- MT5
- CSV
- Database
- API
- Real-time feed
- Backtesting engine
- Other market-data providers

The chart should consume a well-defined market-data abstraction rather than directly depending on a particular external source.

External systems are responsible for adapting their own data and services to this abstraction. The chart module must not contain host-specific adapters unless they are explicitly isolated integration packages outside the chart core.

## Indicators and Analytical Extensions

The architecture must be prepared for future indicators and analytical tools.

Potential future components include:

- Moving averages
- RSI
- MACD
- ADX
- ATR
- Custom indicators
- Multiple indicators
- Indicator panels
- Overlays
- Signals
- Alerts
- Analytical drawings
- Support/resistance
- Trend lines
- Fibonacci tools
- Other financial analysis tools

Do not implement all of these unless required for the initial version.

Instead, design the architecture so that they can be added cleanly later.

## Extensibility

The initial implementation must not create artificial limitations.

Do not design the panel as a feature subset tailored to one known application. Define clean extension points and stable contracts so different host applications can integrate with it without modifying the panel core.

Avoid hard-coded assumptions about:

- Number of candles
- Timeframe
- Symbol
- Data source
- Indicator count
- Chart type
- Screen resolution
- Window size
- Number of panels
- Future analytical features

The system should be designed as a reusable financial chart component rather than a chart created specifically for one test case.

## Reliability

The implementation must handle appropriately:

- Empty datasets
- Missing data
- Duplicate timestamps
- Invalid OHLC data
- Large datasets
- Rapid data updates
- Symbol changes
- Timeframe changes
- Historical loading
- Real-time updates
- Application resizing
- Temporary data-source failures

Do not allow malformed data or unexpected input to destabilize the application.

## Development Process

Do not immediately start modifying files.

First provide:

### Phase 1 — Technical Analysis

Analyze the requirements and determine:

- Appropriate technology
- Appropriate charting library/control
- Architecture
- Rendering approach
- Data model
- Interaction architecture
- Scalability considerations
- Extensibility strategy
- Risks and trade-offs

### Phase 2 — Architecture Proposal

Provide:

- Component structure
- Project/file structure
- Interfaces
- Responsibilities of each component
- Data flow
- Interaction flow

### Phase 3 — Implementation Plan

Divide implementation into small, verifiable stages.

Each stage must:

- Have a clear objective
- Make a limited set of changes
- Be buildable/testable
- Avoid unnecessary modifications to unrelated components

### Phase 4 — Implementation

Only after the architecture and plan are established, implement the system.

After each significant stage:

1. Build the project
2. Check for errors
3. Verify functionality
4. Report what changed
5. Identify the next stage

## Quality Standard

The final result should resemble the architecture of a serious financial/trading application, not a basic chart demo.

Prioritize:

**Correctness → Architecture → Extensibility → Performance → Maintainability → UI polish**

Do not sacrifice architecture merely to make the first prototype appear quickly.

## Critical Requirement

If an existing mature charting component can provide professional financial-chart functionality more reliably than a custom implementation, explicitly identify it and evaluate using it.

Do not assume that writing a chart engine from scratch is preferable.

Likewise, do not select a library merely because it is convenient. Evaluate it against the long-term requirements of a professional, extensible Forex analysis system.

The goal is not simply to display candles.

The goal is to establish a **robust financial chart foundation that can evolve into a complete professional trading-analysis interface.**