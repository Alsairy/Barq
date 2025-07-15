import { useMemo } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../ui/card';

interface MetricDataPoint {
  timestamp: string;
  value: number;
  label?: string;
}

interface MetricsChartProps {
  title: string;
  description?: string;
  data: MetricDataPoint[];
  type?: 'line' | 'bar' | 'area';
  color?: string;
  height?: number;
  showGrid?: boolean;
  formatValue?: (value: number) => string;
}

export function MetricsChart({
  title,
  description,
  data,
  type = 'line',
  color = '#3b82f6',
  height = 200,
  showGrid = true,
  formatValue = (value) => value.toString()
}: MetricsChartProps) {
  const { maxValue, minValue, chartPoints } = useMemo(() => {
    if (data.length === 0) {
      return { maxValue: 0, minValue: 0, chartPoints: [] };
    }

    const values = data.map(d => d.value);
    const max = Math.max(...values);
    const min = Math.min(...values);
    const range = max - min || 1;

    const points = data.map((point, index) => {
      const x = (index / (data.length - 1)) * 100;
      const y = ((max - point.value) / range) * 80 + 10;
      return { x, y, ...point };
    });

    return {
      maxValue: max,
      minValue: min,
      chartPoints: points
    };
  }, [data]);

  const renderLineChart = () => {
    if (chartPoints.length < 2) return null;

    const pathData = chartPoints
      .map((point, index) => `${index === 0 ? 'M' : 'L'} ${point.x} ${point.y}`)
      .join(' ');

    return (
      <g>
        <path
          d={pathData}
          fill="none"
          stroke={color}
          strokeWidth="2"
          className="drop-shadow-sm"
        />
        {chartPoints.map((point, index) => (
          <circle
            key={index}
            cx={point.x}
            cy={point.y}
            r="3"
            fill={color}
            className="hover:r-4 transition-all cursor-pointer"
          >
            <title>{`${point.label || point.timestamp}: ${formatValue(point.value)}`}</title>
          </circle>
        ))}
      </g>
    );
  };

  const renderBarChart = () => {
    const barWidth = Math.max(2, 80 / data.length);
    
    return (
      <g>
        {chartPoints.map((point, index) => (
          <rect
            key={index}
            x={point.x - barWidth / 2}
            y={point.y}
            width={barWidth}
            height={90 - point.y}
            fill={color}
            className="hover:opacity-80 transition-opacity cursor-pointer"
          >
            <title>{`${point.label || point.timestamp}: ${formatValue(point.value)}`}</title>
          </rect>
        ))}
      </g>
    );
  };

  const renderAreaChart = () => {
    if (chartPoints.length < 2) return null;

    const pathData = chartPoints
      .map((point, index) => `${index === 0 ? 'M' : 'L'} ${point.x} ${point.y}`)
      .join(' ');

    const areaPath = `${pathData} L ${chartPoints[chartPoints.length - 1].x} 90 L ${chartPoints[0].x} 90 Z`;

    return (
      <g>
        <path
          d={areaPath}
          fill={color}
          fillOpacity="0.2"
          stroke={color}
          strokeWidth="2"
        />
        {chartPoints.map((point, index) => (
          <circle
            key={index}
            cx={point.x}
            cy={point.y}
            r="2"
            fill={color}
            className="hover:r-3 transition-all cursor-pointer"
          >
            <title>{`${point.label || point.timestamp}: ${formatValue(point.value)}`}</title>
          </circle>
        ))}
      </g>
    );
  };

  const renderChart = () => {
    switch (type) {
      case 'bar':
        return renderBarChart();
      case 'area':
        return renderAreaChart();
      default:
        return renderLineChart();
    }
  };

  const renderGrid = () => {
    if (!showGrid) return null;

    return (
      <g className="opacity-20">
        {[0, 25, 50, 75, 100].map(x => (
          <line
            key={`v-${x}`}
            x1={x}
            y1="10"
            x2={x}
            y2="90"
            stroke="#666"
            strokeWidth="0.5"
          />
        ))}
        {[10, 30, 50, 70, 90].map(y => (
          <line
            key={`h-${y}`}
            x1="0"
            y1={y}
            x2="100"
            y2={y}
            stroke="#666"
            strokeWidth="0.5"
          />
        ))}
      </g>
    );
  };

  if (data.length === 0) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>{title}</CardTitle>
          {description && <CardDescription>{description}</CardDescription>}
        </CardHeader>
        <CardContent>
          <div className="flex items-center justify-center h-48 text-muted-foreground">
            No data available
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>{title}</CardTitle>
        {description && <CardDescription>{description}</CardDescription>}
      </CardHeader>
      <CardContent>
        <div className="relative">
          <svg
            width="100%"
            height={height}
            viewBox="0 0 100 100"
            preserveAspectRatio="none"
            className="border rounded"
          >
            {renderGrid()}
            {renderChart()}
          </svg>
          
          <div className="flex justify-between text-xs text-muted-foreground mt-2">
            <span>{formatValue(minValue)}</span>
            <span>{formatValue(maxValue)}</span>
          </div>
          
          <div className="flex justify-between text-xs text-muted-foreground mt-1">
            <span>{data[0]?.timestamp}</span>
            <span>{data[data.length - 1]?.timestamp}</span>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
