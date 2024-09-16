import { Expansion } from "@angular/compiler";

const MILLIS_PER_SECOND = 1000;
const MILLIS_PER_MINUTE = MILLIS_PER_SECOND * 60;   //     60,000
const MILLIS_PER_HOUR = MILLIS_PER_MINUTE * 60;     //  3,600,000
const MILLIS_PER_DAY = MILLIS_PER_HOUR * 24;        // 86,400,000

export class TimeSpan {
  private _millis: number;

  private static interval(value: number, scale: number): TimeSpan {
    if (Number.isNaN(value)) {
      throw new Error("value can't be NaN");
    }

    const tmp = value * scale;
    const millis = TimeSpan.round(tmp + (value >= 0 ? 0.5 : -0.5));
    if ((millis > TimeSpan.maxValue.totalMilliseconds) || (millis < TimeSpan.minValue.totalMilliseconds)) {
      throw new Error("TimeSpanTooLong");
    }

    return new TimeSpan(millis);
  }

  private static round(n: number): number {
    if (n < 0) {
      return Math.ceil(n);
    } else if (n > 0) {
      return Math.floor(n);
    }

    return 0;
  }

  private static timeToMilliseconds(hour: number, minute: number, second: number): number {
    const totalSeconds = (hour * 3600) + (minute * 60) + second;
    if (totalSeconds > TimeSpan.maxValue.totalSeconds || totalSeconds < TimeSpan.minValue.totalSeconds) {
      throw new Error("TimeSpanTooLong");
    }

    return totalSeconds * MILLIS_PER_SECOND;
  }

  public static get zero(): TimeSpan {
    return new TimeSpan(0);
  }

  public static get maxValue(): TimeSpan {
    return new TimeSpan(Number.MAX_SAFE_INTEGER);
  }

  public static get minValue(): TimeSpan {
    return new TimeSpan(Number.MIN_SAFE_INTEGER);
  }

  public static fromDays(value: number): TimeSpan {
    return TimeSpan.interval(value, MILLIS_PER_DAY);
  }

  public static fromHours(value: number): TimeSpan {
    return TimeSpan.interval(value, MILLIS_PER_HOUR);
  }

  public static fromMilliseconds(value: number): TimeSpan {
    return TimeSpan.interval(value, 1);
  }

  public static fromMinutes(value: number): TimeSpan {
    return TimeSpan.interval(value, MILLIS_PER_MINUTE);
  }

  public static fromSeconds(value: number): TimeSpan {
    return TimeSpan.interval(value, MILLIS_PER_SECOND);
  }

  public static fromTime(hours: number, minutes: number, seconds: number): TimeSpan;
  public static fromTime(days: number, hours: number, minutes: number, seconds: number, milliseconds: number): TimeSpan;
  public static fromTime(daysOrHours: number, hoursOrMinutes: number, minutesOrSeconds: number, seconds?: number, milliseconds?: number): TimeSpan {
    if (milliseconds != undefined) {
      return this.fromTimeStartingFromDays(daysOrHours, hoursOrMinutes, minutesOrSeconds, seconds ?? 0, milliseconds);
    } else {
      return this.fromTimeStartingFromHours(daysOrHours, hoursOrMinutes, minutesOrSeconds);
    }
  }

  private static fromTimeStartingFromHours(hours: number, minutes: number, seconds: number): TimeSpan {
    const millis = TimeSpan.timeToMilliseconds(hours, minutes, seconds);
    return new TimeSpan(millis);
  }

  private static fromTimeStartingFromDays(days: number, hours: number, minutes: number, seconds: number, milliseconds: number): TimeSpan {
    const totalMilliSeconds = (days * MILLIS_PER_DAY) +
      (hours * MILLIS_PER_HOUR) +
      (minutes * MILLIS_PER_MINUTE) +
      (seconds * MILLIS_PER_SECOND) +
      milliseconds;

    if (totalMilliSeconds > TimeSpan.maxValue.totalMilliseconds || totalMilliSeconds < TimeSpan.minValue.totalMilliseconds) {
      throw new Error("TimeSpanTooLong");
    }
    return new TimeSpan(totalMilliSeconds);
  }

  constructor(millis: number) {
    this._millis = millis;
  }

  public get days(): number {
    return TimeSpan.round(this._millis / MILLIS_PER_DAY);
  }

  public get hours(): number {
    return TimeSpan.round((this._millis / MILLIS_PER_HOUR) % 24);
  }

  public get minutes(): number {
    return TimeSpan.round((this._millis / MILLIS_PER_MINUTE) % 60);
  }

  public get seconds(): number {
    return TimeSpan.round((this._millis / MILLIS_PER_SECOND) % 60);
  }

  public get milliseconds(): number {
    return TimeSpan.round(this._millis % 1000);
  }

  public get totalDays(): number {
    return this._millis / MILLIS_PER_DAY;
  }

  public get totalHours(): number {
    return this._millis / MILLIS_PER_HOUR;
  }

  public get totalMinutes(): number {
    return this._millis / MILLIS_PER_MINUTE;
  }

  public get totalSeconds(): number {
    return this._millis / MILLIS_PER_SECOND;
  }

  public get totalMilliseconds(): number {
    return this._millis;
  }

  public add(ts: TimeSpan): TimeSpan {
    const result = this._millis + ts.totalMilliseconds;
    return new TimeSpan(result);
  }

  public subtract(ts: TimeSpan): TimeSpan {
    const result = this._millis - ts.totalMilliseconds;
    return new TimeSpan(result);
  }

  public toString(): string {
    const msFormatted = Math.abs(this.milliseconds).toString().padStart(3, '0').padEnd(7, '0');

    // Construct the TimeSpan string
    const timeSpanString =
      `${this.totalMilliseconds < 0 ? '-' : ''}` +
      `${Math.abs(this.days) > 0 ? `${Math.abs(this.days)}.` : ''}` +
      `${Math.abs(this.hours).toString().padStart(2, '0')}:` +
      `${Math.abs(this.minutes).toString().padStart(2, '0')}:` +
      `${Math.abs(this.seconds).toString().padStart(2, '0')}` +
      (Math.abs(this.milliseconds) > 0 ? `.${msFormatted}` : '');

    return timeSpanString;
  }

  public static fromJSON(value: string | TimeSpan): TimeSpan {
    if (value instanceof TimeSpan) {
      return value;
    }

    return TimeSpan.fromString(value);
  }

  public static fromString(value: string): TimeSpan {
    // Regex to parse days, hours, minutes, seconds, and milliseconds
    const timeSpanRegex = /^(?<MULTIPLIER>-)?((?<DAYS>\d{1,})\.)?(?<HOURS>\d{2}):(?<MINUTES>\d{2}):(?<SECONDS>\d{2})(\.(?<MS>\d{7}))?$/;
    const match = value.match(timeSpanRegex);

    if (!match) {
      throw new Error('Invalid TimeSpan format');
    }

    // Extract values from the matched regex groups
    const { MULTIPLIER, DAYS, HOURS, MINUTES, SECONDS, MS } = match.groups || {};

    // Default values if any group is missing
    const multiplier = MULTIPLIER ? -1 : 1;
    const days = DAYS || '0';
    const hours = HOURS || '00';
    const minutes = MINUTES || '00';
    const seconds = SECONDS || '00';
    const milliseconds = MS ? MS.substring(0, 3) : '0'; // Extract milliseconds up to 3 digits

    return TimeSpan.fromTime(
      parseInt(days, 10) * multiplier,
      parseInt(hours, 10) * multiplier,
      parseInt(minutes, 10) * multiplier,
      parseInt(seconds, 10) * multiplier,
      parseInt(milliseconds, 10) * multiplier
    );
  }

  toJSON(): string {
    return this.toString();
  }
}
