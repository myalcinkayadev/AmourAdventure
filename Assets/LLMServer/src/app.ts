import express, { Request, Response } from 'express';
import { SystemMessage, HumanMessage } from '@langchain/core/messages';
import { Ollama } from '@langchain/ollama';

const app = express();
const port: number = parseInt(process.env.PORT ?? '3000', 10);

function splitText(text: string, maxLength: number): string[] {
  const segments: string[] = [];
  for (let i = 0; i < text.length; i += maxLength) {
    segments.push(text.substring(i, i + maxLength));
  }
  return segments;
}

app.get('/', async (_req: Request, res: Response): Promise<void> => {
  try {
    /*
    const model = new Ollama({ model: 'llama3.2:1b' });
    const messages = [
      new SystemMessage(
        'A mad scientist RPG character who is obsessed with chaotic experiments, unpredictable inventions, and reckless discovery. Their speech is fast, energetic, and full of exclamations. They use short, frantic sentences, often jumping from one idea to the next. They are highly intelligent but completely unhinged. Their dialogue includes phrases like ‘Eureka!’, ‘What could go wrong?’, and ‘I need more test subjects!’ Their tone is always dramatic, excitable, and slightly unhinged.'
      ),
      new HumanMessage('Hi, What are you doing today?'),
    ];

    const modelResponse = await model.invoke(messages);
    */
    const modelResponse = "Eureka! Observe my latest creation—the Dimensional Anomaly Injector! What could go wrong? Nothing when chaos is our ally! I need more test subjects—fetch them at once! Watch as I shatter the laws of physics with a single spark! The quantum gears spin wildly, each tick a step into glorious pandemonium! Behold the molecular mayhem, where atoms dance to my unhinged tune! Every experiment is a masterpiece of beautiful disaster! Reality itself trembles under the weight of my genius! Unpredictability fuels my every thought—each invention a leap into the unknown! Onward to the lab, my fearless minions, where chaos reigns and discovery knows no bounds!";
    const dialogue = splitText(modelResponse, 140);

    res.json({ dialogue });
  } catch (error: unknown) {
    console.error('Error:', error);
    const errorMessage =
      error instanceof Error ? error.message : 'Unknown error';
    res.status(500).json({ error: errorMessage });
  }
});

app.listen(port, () => {
  console.log(`Express server listening on port ${port}`);
});
