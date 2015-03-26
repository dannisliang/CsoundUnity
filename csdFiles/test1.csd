<CsoundSynthesizer>
<CsOptions>
 -+rtaudio=winmm -odac -m0d -B64
</CsOptions>
<CsInstruments>
sr=44100
ksmps=32
nchnls=2
0dbfs=1

instr 1 
k1 expon 1, p3, 0.001
a1 oscili 1, 440, -1
outs a1*k1, a1*k1
endin


instr 2
kTrig chnget "trigger"
if changed:k(kTrig)==1 then
printks "value changed", 0.1
event "i", 1, 0, 1
endif
endin


</CsInstruments>

<CsScore>
i2 0 1000
</CsScore>

</CsoundSynthesizer>
