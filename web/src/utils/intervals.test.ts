import { describe, expect, it } from 'vitest'
import { toStateIntervals } from './intervals'

const points = (values: number[]) => values.map((value, time) => ({ time, value }))

describe('toStateIntervals', () => {
  it('regroupe les mesures consécutives de même état', () => {
    expect(toStateIntervals(points([1, 1, 0, 0, 0, 1]))).toEqual([
      { start: 0, end: 1, isOn: true },
      { start: 2, end: 4, isOn: false },
      { start: 5, end: 5, isOn: true },
    ])
  })

  it('renvoie une seule période quand l’état ne change jamais', () => {
    expect(toStateIntervals(points([0, 0, 0]))).toEqual([{ start: 0, end: 2, isOn: false }])
  })

  it('renvoie une liste vide sans mesure', () => {
    expect(toStateIntervals([])).toEqual([])
  })
})
