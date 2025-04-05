# Format

## Index

1. [Overview](#overview)
2. [Methods](#methods)
   - [FormatTimeSpan](#formattimespantimespan-timespan)
   - [FormatPercentage](#formatpercentage-double-value)
   - [FormatDate](#formatdate-datetime-datetime-string-format-null)
   - [FormatShortDate](#formatshortdate-datetime-datetime)
   - [FormatDurationMinutes](#formatdurationminutestimespan-timespan)
   - [FormatDurationHours](#formatdurationhourstimespan-timespan)
   - [FormatWithSign](#formatwithsign-double-value-string-suffix-null)
   - [FormatTitleCase](#formattitlecase-string-input)
   
## Overview

The `Format` class provides various utility methods for formatting common types of data such as `TimeSpan`, `DateTime`, and `double` values into user-friendly string representations. These formatting methods ensure that values are formatted consistently, with options for customizing how data appears (e.g., including a plus or minus sign for values, or adjusting the time format).

## Methods

### FormatTimeSpan(TimeSpan timeSpan)

Formats a `TimeSpan` object into a string in the format `HH:mm:ss`.

**Parameters:**

- `timeSpan` (TimeSpan): The `TimeSpan` object to format.

**Returns:**

- `string`: The formatted time span as `HH:mm:ss`.

---

### FormatPercentage(double value)

Formats a `double` value as a percentage with one decimal place.

**Parameters:**

- `value` (double): The value to format as a percentage.

**Returns:**

- `string`: The formatted percentage, e.g., `25.5%`.

---

### FormatDate(DateTime dateTime, string? format = null)

Formats a `DateTime` object into a string using the provided format or a default format if no format is specified.

**Parameters:**

- `dateTime` (DateTime): The `DateTime` object to format.
- `format` (string?, optional): An optional format string. If not provided, a default format of `yyyy-MM-dd HH:mm` is used.

**Returns:**

- `string`: The formatted date string.

---

### FormatShortDate(DateTime dateTime)

Formats a `DateTime` object into a short date format (e.g., `5 Apr 2025`).

**Parameters:**

- `dateTime` (DateTime): The `DateTime` object to format.

**Returns:**

- `string`: The short date formatted string, e.g., `5 Apr 2025`.

---

### FormatDurationMinutes(TimeSpan timeSpan)

Formats a `TimeSpan` into the total duration in minutes.

**Parameters:**

- `timeSpan` (TimeSpan): The `TimeSpan` object to format.

**Returns:**

- `string`: The formatted duration in minutes, e.g., `120 min`.

---

### FormatDurationHours(TimeSpan timeSpan)

Formats a `TimeSpan` into the total duration in hours, rounded to one decimal place.

**Parameters:**

- `timeSpan` (TimeSpan): The `TimeSpan` object to format.

**Returns:**

- `string`: The formatted duration in hours, e.g., `2.5 h`.

---

### FormatWithSign(double value, string? suffix = null)

Formats a `double` value with a sign (+ or -) and an optional suffix.

**Parameters:**

- `value` (double): The value to format.
- `suffix` (string?, optional): An optional suffix to append to the value (e.g., "kg").

**Returns:**

- `string`: The formatted value with a sign and optional suffix, e.g., `+3.2 kg` or `-5.0`.

---

### FormatTitleCase(string input)

Formats a string to title case, where the first letter of each word is capitalized.

**Parameters:**

- `input` (string): The input string to format.

**Returns:**

- `string`: The formatted string in title case, e.g., `Hello World`.
