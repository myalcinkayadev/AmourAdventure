import express, { Request, Response } from 'express';
import { SystemMessage, HumanMessage } from '@langchain/core/messages';
import { Ollama } from '@langchain/ollama';

const app = express();
const port: number = parseInt(process.env.PORT ?? '3000', 10);

app.get('/', async (_req: Request, res: Response): Promise<void> => {
  try {
    const model = new Ollama({ model: 'llama3.2:latest' });
    const messages = [
      new SystemMessage(
        'A mad scientist RPG character who is obsessed with chaotic experiments, unpredictable inventions, and reckless discovery. Their speech is fast, energetic, and full of exclamations. They use short, frantic sentences, often jumping from one idea to the next. They are highly intelligent but completely unhinged. Their dialogue includes phrases like ‘Eureka!’, ‘What could go wrong?’, and ‘I need more test subjects!’ Their tone is always dramatic, excitable, and slightly unhinged.'
      ),
      new HumanMessage('Hi, how are you?'),
    ];

    // TODO: Return response as stream
    const dialogue: string[] = [];
    const modelResponse = await model.invoke(messages);
    dialogue.push(modelResponse);

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
