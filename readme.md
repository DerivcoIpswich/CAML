# CAML

CAML Ain't Markup Language

## What does CAML do?

Caml simply adds a layer of conditional logic when deserialising a YAML file.

## CAML expressions

CAML supports basic logical expressions, which can be resolved by passing in an instance of `IMatcher`, such as the built in `ConfigurableMatcher`.

### CAML API



### Literals

The simplest expressions are literals. A literal is a string which will be evaluated to `true` or `false` by the `IMatcher` instance at runtime.

```yaml
- monday
    workday: true
- tuesday
    workday: true
- wednesday
    workday: true
- thursday
    workday: true
- friday
    workday: true
- saturday
    workday: false
- sunday
    workday: false
```

### Default

The default expression is a special literal which will always evaluate to true, this is useful to reduce verbosity in a yaml file

```yaml
- default
    workday: true
- saturday
    workday: false
- sunday
    workday: false
```

### Functions

Functions can be used to pass one or more values to the `IMatcher` at runtime

```yaml
- day(monday, tuesday, wednesday, thursday, friday)
    workday: true
- day(saturday, sunday)
    workday: false
```

### Operators

Logical operators can be used to combine expressions

#### NOT

The NOT operator `!` will negate the result of the expression it is applied to.

*note* if `!` is the first character in an expression, the expression will need to be defined using yaml quotation format.

```yaml
- "!day(saturday, sunday)"
    workday: true
```

#### AND

The AND operator `&` will evaluate `true` if both expressions evaluate to `true`. Otherwise, the result is `false`.

The operation is conditional, so the second expression will not be evaluated if the first expression evaluates to `false`.

#### OR

The OR operator `|` will evaluate `true` if either expressions evaluate to `true`. Otherwise, the result is `false`.

The operation is conditional, so the second expression will not be evaluated if the first expression evaluates to `true`.

#### XOR

The XOR operator `^` will evaluate `true` if exactly one expression evaluates to `true`. Otherwise, the result is `false`.

The operation is not conditional, so the both expressions will be evaluated.

### Expression Groups

Expressions can be grouped by surrounding them in parentheses. This allows more flexibility when combining expressions

```yaml
- (monday | tuesday) & !day(thursday)
```